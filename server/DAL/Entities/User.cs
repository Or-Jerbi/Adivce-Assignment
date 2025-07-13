﻿using System;
using System.Collections.Generic;

namespace AdviceAssignement.DAL.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
}
