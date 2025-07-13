namespace AdviceAssignement.DTOs
{
    public class ElevatorCallDto
    {
        public int BuildingId { get; set; }

        public int RequestedFloor { get; set; }

        public int? DestinaionFloor { get; set; }

        public int Direction { get; set; }
    }
}
