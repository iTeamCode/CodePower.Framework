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
    /// abstract aspect attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class AspectAttribute : Attribute
    {
        public Type CallHandlerType { get; protected set; }

        protected abstract PropertyInfo[] PropertiesInfo { get; }

        public NameValueCollection GetAttrs()
        {
            NameValueCollection attrs = new NameValueCollection();

            foreach (var p in PropertiesInfo)
            {
                object value = p.GetValue(this, null);
                if (value == null)
                {
                    attrs.Add(p.Name, string.Empty);
                }
                else
                {
                    attrs.Add(p.Name, value.ToString());
                }
                
            }

            return attrs;
        }
    }
}
