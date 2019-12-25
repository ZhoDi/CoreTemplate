using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application.Dto
{
    public abstract class Dto<TPrimaryKey> :IDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }

    public class Dto : Dto<int>
    {

    }
}
