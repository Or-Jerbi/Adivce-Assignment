import React from "react";
import { Link } from "react-router-dom";
import "./styles.css";

function BuildingCard({ building }) {
  return (
    <Link to={`/building/${building.id}`} className="building-card">
      <h5>{building.name}</h5>
      <h6>{building.numberOfFloors} Floors</h6>
    </Link>
  );
}

export default BuildingCard;
