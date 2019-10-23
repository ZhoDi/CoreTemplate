using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        //public virtual bool IsTransient()
        //{
        //    if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
        //    {
        //        return true;
        //    }

        //    //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
        //    if (typeof(TPrimaryKey) == typeof(int))
        //    {
        //        return Convert.ToInt32(Id) <= 0;
        //    }

        //    if (typeof(TPrimaryKey) == typeof(long))
        //    {
        //        return Convert.ToInt64(Id) <= 0;
        //    }

        //    return false;
        //}
    }

    /// <summary>
    /// 定义默认主键类型为Guid的实体基类
    /// </summary>
    public class Entity : Entity<Guid>
    {

    }
}
