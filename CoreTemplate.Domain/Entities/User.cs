using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CoreTemplate.Domain.Entities.Base;

namespace CoreTemplate.Domain.Entities
{
    public class User : Entity<int>
    {
        /// <summary>
        /// 用户登录Id
        /// </summary>
        [Column(TypeName = "varchar(10)")]
        public string LoginId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Column(TypeName = "varchar(10)")]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string Mobile { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Column(TypeName = "int(1)")]
        public int Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string Email { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string Avatar { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string Password { get; set; }
    }
}
