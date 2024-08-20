﻿using Domain.Aggregates;

namespace Domain.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<LeaveType>? LeaveTypes { get; set; } = new List<LeaveType>();
    }
}