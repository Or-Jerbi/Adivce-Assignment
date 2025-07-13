namespace AdviceAssignement.DAL.Entities
{
    public class Enums
    {
        public enum ElevatorStatus
        {
            Idle,
            MovingUp,
            MovingDown,
            OpeningDoors,
            ClosingDoors
        }

        public enum ElevatorDirection
        {
            None,
            Up,
            Down
        }

        public enum DoorStatus
        {
            Closed,
            Open
        }
    }
}
