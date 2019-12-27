using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain
{
    public class PageModel<TEntity>
        where TEntity : class
    {
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<TEntity> TEntityList { get; set; }
    }
}
