using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application.Application
{
    public class NullObjectMapper : IObjectMapper
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullObjectMapper Instance { get; } = new NullObjectMapper();


        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException("IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TDestination>(object source)
        {
            throw new NotImplementedException("IObjectMapper should be implemented in order to map objects");
        }
    }
}
