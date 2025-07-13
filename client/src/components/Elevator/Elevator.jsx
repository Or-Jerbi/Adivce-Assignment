import { React, useEffect, useState } from "react";
import "./styles.css";

function Elevator({ status, direction }) {
  const [stringStatus, setStringStatus] = useState();
  const [strinDirection, setStringdirection] = useState();

  useEffect(() => {
    function setStatusToString() {
      switch (status) {
        case 0:
          setStringStatus("Idle");
          break;
        case 1:
          setStringStatus("Moving Up");
          break;
        case 2:
          setStringStatus("Moving Down");
          break;
        case 3:
          setStringStatus("Doors Open");
          break;
        case 4:
          setStringStatus("Closing Doors");
          break;
        default:
          break;
      }
    }

    function setDirectionToString() {
      switch (direction) {
        case 0:
          setStringdirection("None");
          break;
        case 1:
          setStringdirection("Up");
          break;
        case 2:
          setStringdirection("Down");
          break;
        default:
          break;
      }
    }

    setStatusToString();
    setDirectionToString();
  }, [status, direction]);
  return (
    <div className="elevator">
      <div>{stringStatus}</div>
      <div>{strinDirection}</div>
    </div>
  );
}

export default Elevator;
