import React from "react";
import { Link } from "react-router-dom";
import "./styles.css";

function BuildingCard({ building }) {
  return (
    <Link to={`/building/${building.id}`} className="building-card">
      <div>{building.name}</div>
      <div>{building.numberOfFloors}</div>
    </Link>
  );
}

export default BuildingCard;
