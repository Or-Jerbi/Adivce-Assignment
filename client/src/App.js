import "./App.css";
import Register from "./pages/Register/registerPage";
import BuildingManagement from "./pages/BuildingManagement/buildingManagementPage";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/Login/LoginPage";
import BuildingPage from "./pages/BuildingPage/BuildingPage";

function App() {
  return (
    <BrowserRouter>
      <div className="App">
        <Routes>
          <Route path="/" element={<LoginPage />} />
          <Route path="/register" element={<Register />} />
          <Route path="/profile" element={<BuildingManagement />} />
          <Route path="/building/:id" element={<BuildingPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
