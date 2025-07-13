using System;
using System.Collections.Generic;

namespace AdviceAssignement.DAL.Entities;

public partial class Elevator
{
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public int CurrentFloor { get; set; }

    public int Status { get; set; }

    public int Direction { get; set; }

    public int DoorStatus { get; set; }

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<ElevatorCallAssignment> ElevatorCallAssignments { get; set; } = new List<ElevatorCallAssignment>();
}
