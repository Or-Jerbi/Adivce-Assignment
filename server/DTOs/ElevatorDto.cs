using AdviceAssignement.DAL.Entities;

namespace AdviceAssignement.DTOs
{
    public class ElevatorDto
    {
        public int BuildingId { get; set; }

        public int CurrentFloor { get; set; }
        public int? Status { get; set; }

        public int? Direction { get; set; }

        public int? DoorStatus { get; set; }

    }

}

