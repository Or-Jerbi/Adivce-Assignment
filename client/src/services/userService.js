import { postWithData } from "./baseService";
import { get } from "./baseService";

const controllerUrl = "/User";

export function createUser(email, password) {
  return postWithData(controllerUrl + "/CreateUser", { email, password });
}

export function getAllusers() {
  return get(controllerUrl + "/GetAllUsers");
}

export function Login(email, password) {
  return postWithData(controllerUrl + "/Login", { email, password });
}
