import axios from "axios";

axios.defaults.baseURL = process.env.REACT_APP_BACKEND_URL + "/api";

export const get = async (url) => {
  try {
    return await axios.get(url);
  } catch (error) {
    console.error("Error fetching data:", error);
    throw error;
  }
};

export const getWithHeaders = async (url, headers = {}) => {
  try {
    return await axios.get(url, { headers: headers });
  } catch (error) {
    console.error("Error fetching data:", error);
    throw error;
  }
};

export const postWithData = async (url, data, headers = {}) => {
  try {
    return await axios.post(url, data, { headers: headers });
  } catch (error) {
    console.error("Error posting data:", error);
    throw error;
  }
};

export const authHeader = () => {
  const token = localStorage.getItem("jwtToken");
  if (!token) throw new Error("No token found");
  return { Authorization: `Bearer ${token}` };
};
