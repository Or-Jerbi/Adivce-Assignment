import { React, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Login } from "../../services/userService";
import "./styles.css";

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await Login(email, password);
      if (!response.data) {
        throw new Error("Login failed");
      }
      const data = await response.data;
      console.log(data);
      localStorage.setItem("jwtToken", data.token);
      navigate("/profile");
    } catch (err) {
      console.log(err);
    }
  };
  return (
    <form onSubmit={handleSubmit} className="form-container">
      <h1>Login</h1>
      <input
        type="email"
        onChange={(e) => setEmail(e.target.value)}
        placeholder="*Email"
        required
      ></input>
      <input
        type="password"
        onChange={(e) => setPassword(e.target.value)}
        placeholder="*Password"
        required
      ></input>
      <div>
        <span>Dont have an account? </span>
        <Link to="/register">Register</Link>
      </div>
      <button type="submit">Submit</button>
    </form>
  );
}

export default LoginPage;
