import { React, useEffect, useState } from "react";
import { GetBuildingsByUserId } from "../../services/buildingService";
import BuildingCard from "../BuildingCard/BuildingCard";
import "./styles.css";

function BuildingsDisplay() {
  const [buildingList, setBuildingList] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const buildingsList = await GetBuildingsByUserId();
        if (buildingsList.data) {
          setBuildingList(buildingsList.data);
          console.log(buildingsList.data);
        } else {
          alert("user id incorrect");
        }
      } catch (e) {
        console.error(e);
      }
    }
    fetchData();
  }, []);
  return (
    <div className="container">
      <div>Building Management</div>
      <div className="building-grid">
        {buildingList &&
          buildingList.map((b) => <BuildingCard key={b.id} building={b} />)}
      </div>
    </div>
  );
}

export default BuildingsDisplay;
