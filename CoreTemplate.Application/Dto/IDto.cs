using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application.Dto
{
    public interface IDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }

    public interface IDto: IDto<int>
    {

    }
}
