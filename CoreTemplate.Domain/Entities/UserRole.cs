using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Domain.Entities.Base;

namespace CoreTemplate.Domain.Entities
{
    public class UserRole: Entity<int>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
