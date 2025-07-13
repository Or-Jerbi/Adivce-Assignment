import { getWithHeaders, postWithData } from "./baseService";

const controllerUrl = "/ElevatorCall";

export function getAllElevatorCalls(buildingId) {
  return getWithHeaders(controllerUrl + "/GetAllElevatorCalls", {
    buildingId: buildingId,
  });
}

export function createElevatorCall(
  buildingId,
  requestedFloor,
  destinationFloor,
  direction
) {
  return postWithData(controllerUrl + "/CreateElevatorCall", {
    buildingId: buildingId,
    requestedFloor: requestedFloor,
    destinaionFloor: destinationFloor,
    direction: direction,
  });
}
