using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemplate.Domain.Entities
{
    public class Role : Entity<int>
    {
        [MaxLength(10)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public long CreateDate { get; set; }
    }
}
