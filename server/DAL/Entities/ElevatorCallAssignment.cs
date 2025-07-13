using System;
using System.Collections.Generic;

namespace AdviceAssignement.DAL.Entities;

public partial class ElevatorCallAssignment
{
    public int Id { get; set; }

    public int ElevatorId { get; set; }

    public DateTime AssignmentTime { get; set; }

    public virtual Elevator Elevator { get; set; } = null!;
}
