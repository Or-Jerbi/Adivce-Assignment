import { React, useState } from "react";
import { CreateNewBuilding } from "../../services/buildingService";
import { useNavigate } from "react-router-dom";
import "./styles.css";

function CreateBuilding() {
  const [name, setName] = useState("");
  const [numberOfFloors, setNumberOfFloors] = useState("");
  const navigate = useNavigate();

  const HandleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await CreateNewBuilding(name, parseInt(numberOfFloors));
      if (response.data) {
        console.log(response.data);
        navigate(`/building/${response.data.id}`);
      }
    } catch (error) {
      alert("couldnt create new building");
      console.error(error);
    }
  };
  return (
    <form className="form-container">
      <div>
        <div>Create new building</div>
        <input
          type="text"
          placeholder="*Building Name"
          onChange={(e) => setName(e.target.value)}
          required
        ></input>
        <input
          type="number"
          placeholder="*Floor's number"
          onChange={(e) => setNumberOfFloors(e.target.value)}
          required
        ></input>
      </div>
      <button onClick={HandleSubmit}>Create</button>
    </form>
  );
}

export default CreateBuilding;
