import { getWithHeaders, postWithData, authHeader } from "./baseService";

const controllerUrl = "/Building";

export function GetBuildingById(id) {
  return getWithHeaders(controllerUrl + "/GetBuildingById", { id });
}

export function CreateNewBuilding(name, numberOfFloors) {
  return postWithData(
    controllerUrl + "/CreateBuilding",
    { name, numberOfFloors },
    authHeader()
  );
}

export function GetBuildingsByUserId() {
  return getWithHeaders(controllerUrl + "/GetUserBuildings", authHeader());
}
