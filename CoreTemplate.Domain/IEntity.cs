using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain
{
    /// <summary>
    /// 定义含有主键Id的实体类接口
    /// </summary>
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        //bool IsTransient();
    }

    public interface IEntity : IEntity<int>
    {

    }
}
