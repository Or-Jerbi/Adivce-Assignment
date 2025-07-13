/**
 * @typedef {number} ElevatorStatusEnum
 * @enum {ElevatorStatusEnum}
 */
export const ElevatorStatus = {
  Idle: 0,
  MovingUp: 1,
  MovingDown: 2,
  OpeningDoors: 3,
  ClosingDoors: 4,
};

/**
 * @typedef {number} ElevatorDirectionEnum
 * @enum {ElevatorDirectionEnum}
 */
export const ElevatorDirection = {
  None: 0,
  Up: 1,
  Down: 2,
};

/**
 * @typedef {number} DoorStatusEnum
 * @enum {DoorStatusEnum}
 */
export const DoorStatus = {
  Closed: 0,
  Open: 1,
};
