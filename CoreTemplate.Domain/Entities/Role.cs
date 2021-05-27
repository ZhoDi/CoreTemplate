using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CoreTemplate.Domain.Entities.Base;

namespace CoreTemplate.Domain.Entities
{
    public class Role : Entity<int>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Column(TypeName = "varchar(10)")]
        public string Name { get; set; }
    }
}
