import React from "react";
import { useState } from "react";
import { createUser } from "../../services/userService";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import "./styles.css";

const Register = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();
  const onClicked = async (e) => {
    e.preventDefault();
    try {
      const response = await createUser(email, password);
      if (response.data) {
        console.log(response.data);
        navigate("/");
      }
    } catch (e) {
      alert("Email is already exist");
      console.error(e);
    }
  };
  return (
    <form onSubmit={onClicked} className="form-container">
      <h1>Register</h1>
      <input
        type="email"
        placeholder="*Email"
        onChange={(e) => setEmail(e.target.value)}
        required
      ></input>
      <input
        type="password"
        placeholder="*Password"
        onChange={(e) => setPassword(e.target.value)}
        required
      ></input>
      <div>
        <span>Have an account? </span>
        <Link to="/">Login</Link>
      </div>
      <button type="submit">Submit</button>
    </form>
  );
};

export default Register;
