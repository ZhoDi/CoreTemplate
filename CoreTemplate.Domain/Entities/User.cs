using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemplate.Domain.Entities
{
    public class User : Entity<int>
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(15)]
        public string Mobile { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(30)]
        public string Email { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [MaxLength(20)]
        public string Number { get; set; }

        [MaxLength(20)]
        public string PassWord { get; set; }

        public long CreateDate { get; set; }

        /// <summary>
        /// 是否注销
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
