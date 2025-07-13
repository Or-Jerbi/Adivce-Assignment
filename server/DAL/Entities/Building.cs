using System;
using System.Collections.Generic;

namespace AdviceAssignement.DAL.Entities;

public partial class Building
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int NumberOfFloors { get; set; }

    public virtual Elevator? Elevator { get; set; }

    public virtual ICollection<ElevatorCall> ElevatorCalls { get; set; } = new List<ElevatorCall>();

    public virtual User User { get; set; } = null!;
}
