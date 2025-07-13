import { useState, useEffect, useRef } from "react";
import { useParams } from "react-router-dom";
import { GetBuildingById } from "../../services/buildingService";
import { createElevatorCall } from "../../services/elevatorCallService";
import { ElevatorDirection, ElevatorStatus } from "../../enums";
import Elevator from "../../components/Elevator/Elevator";
import * as signalR from "@microsoft/signalr";
import "./styles.css";

function BuildingPage() {
  const [building, setBuilding] = useState([]);
  const [status, setStatus] = useState("");
  const [currentDirection, setDirection] = useState(ElevatorDirection.None);
  const { id } = useParams();
  const floors = Array.from(
    { length: building.numberOfFloors },
    (_, i) => building.numberOfFloors - i - 1
  );
  const [currentFloor, setCurrentFloor] = useState(0);

  const OrderElevator = async (requestedFloor, destinationFloor, direction) => {
    try {
      var res = await createElevatorCall(
        id,
        requestedFloor,
        destinationFloor,
        direction
      );
      if (!res.data) {
        throw new Error("Failed to create call");
      }
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    const getBuilding = async () => {
      try {
        const res = await GetBuildingById(id);
        if (!res.data) {
          throw new Error("Failed to fetch building");
        }
        setBuilding(res.data);
      } catch (err) {
        console.error(err);
      }
    };
    getBuilding();
  }, [id]);

  const connectionRef = useRef(null);

  useEffect(() => {
    const connectToHub = async () => {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl(process.env.REACT_APP_BACKEND_URL + "/elevatorHub")
        .withAutomaticReconnect()
        .build();

      connection.on("ElevatorUpdated", (data) => {
        console.log("Elevator updated:", data);
        setCurrentFloor(data.currentFloor);
        setStatus(data.status);
        setDirection(data.direction);
      });

      connection.on("SimulationStarted", (id) => {
        console.log(`Simulation started for elevator ${id}`);
      });

      connection.on("Error", (message) => {
        console.error("Simulation error:", message);
      });

      try {
        await connection.start();
        console.log("Connected to elevator hub");
        connectionRef.current = connection;
        if (id) {
          connectionRef.current
            .invoke("StartSimulation", Number(id))
            .then(() => console.log(`Requested simulation for elevator ${id}`));
        }
      } catch (err) {
        console.error("Failed to connect to hub:", err);
      }
    };

    connectToHub();
    return () => {
      if (
        connectionRef.current &&
        connectionRef.current.state === signalR.HubConnectionState.Connected
      ) {
        connectionRef.current
          .invoke("StopSimulation", Number(id))
          .catch((err) =>
            console.error("error in StopSimulation request:", err)
          );
      } else {
        console.log(`connection lost, cannot send StopSimulation request`);
      }

      connectionRef.current?.stop();
      console.log("Elevator Hub disconnected");
    };
  }, [id]);

  return (
    <div className="building">
      {floors &&
        floors.map((floor) => (
          <div className="floor" key={floor}>
            <button
              disabled={
                status !== ElevatorStatus.OpeningDoors ||
                (currentDirection === ElevatorDirection.Up &&
                  floor <= currentFloor) ||
                (currentDirection === ElevatorDirection.Down &&
                  floor >= currentFloor)
              }
              onClick={() =>
                OrderElevator(currentFloor, floor, currentDirection)
              }
              className="floor-number"
            >
              Floor {floor}
            </button>
            {currentFloor === floor && (
              <Elevator status={status} direction={currentDirection} />
            )}
            <div className="elevator-buttons">
              <button
                onClick={() => OrderElevator(floor, null, ElevatorDirection.Up)}
                disabled={floor === floors.length - 1}
              >
                ▲
              </button>
              <button
                onClick={() =>
                  OrderElevator(floor, null, ElevatorDirection.Down)
                }
                disabled={floor === 0}
              >
                ▼
              </button>
            </div>
          </div>
        ))}
    </div>
  );
}

export default BuildingPage;
