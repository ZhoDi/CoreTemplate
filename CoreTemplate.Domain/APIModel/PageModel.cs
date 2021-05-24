using System.Collections.Generic;

namespace CoreTemplate.Domain.APIModel
{
    public class PageModel<T>
        where T : class
    {
        public long TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
