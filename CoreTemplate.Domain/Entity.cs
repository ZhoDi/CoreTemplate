using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain
{
    public abstract class Entity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// 定义默认主键类型为Guid的实体基类
    /// </summary>
    public class Entity : Entity<int>
    {

    }
}
