using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheAttribute : AspectAttribute
    {
        public CacheAttribute()
        {
            CallHandlerType = typeof(CacheCallHandler);
        }

        public string CacheKey { get; set; }

        public int DurationMinutes { get; set; }


        private readonly static PropertyInfo[] Properties = typeof(CacheAttribute).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        protected override PropertyInfo[] PropertiesInfo
        {
            get { return Properties; }
        }
    }
}
