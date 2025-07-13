using System;
using System.Collections.Generic;

namespace AdviceAssignement.DAL.Entities;

public partial class ElevatorCall
{
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public int RequestedFloor { get; set; }

    public int? DestinaionFloor { get; set; }

    public DateTime CallTime { get; set; }

    public bool IsHandled { get; set; }

    public virtual Building Building { get; set; } = null!;
}
