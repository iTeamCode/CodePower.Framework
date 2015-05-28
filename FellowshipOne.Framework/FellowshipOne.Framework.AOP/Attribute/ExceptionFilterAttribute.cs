using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    /// <summary>
    /// Exception Filter
    /// </summary>
    public class ExceptionFilterAttribute : AspectAttribute
    {
        /// <summary>
        /// 构造函数：初始化 CallHandlerType
        /// </summary>
        public ExceptionFilterAttribute()
        {
            CallHandlerType = typeof(ExceptionFilterHandler);
        }

        private readonly static PropertyInfo[] Properties = typeof(ExceptionFilterAttribute).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        protected override PropertyInfo[] PropertiesInfo
        {
            get { return Properties; }
        }
    }
}
