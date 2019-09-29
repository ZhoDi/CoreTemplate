using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain.Entities
{
    public class UserRole: Entity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
