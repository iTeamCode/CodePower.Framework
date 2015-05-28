using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    /// <summary>
    /// 异常过滤器：处理程序 - Exception Filter Handler
    /// </summary>
    public class ExceptionFilterHandler : ICallHandler
    {

        /// <summary>
        /// 构造函数 - Constructor Function
        /// </summary>
        /// <param name="attributes">attribute collection. </param>
        public ExceptionFilterHandler(NameValueCollection attributes)
        {
        }
        /// <summary>
        /// 函数调用前的处理：检查函数返回类型是否正确
        /// BeginInvoke：Check return type of function.
        /// </summary>
        /// <param name="context">context object</param>
        public void BeginInvoke(MethodContext context)
        {
            if (context == null) return;
            if (context.ReturnType == null) return;

            var returnType = context.ReturnType;
            if (!returnType.IsSubclassOf(typeof(ResultEntity)) && !returnType.Equals(typeof(ResultEntity)))
            {
                throw new Exception("具有ExceptionFilter属性的方法，必须以继承自类 ResultEntity 的对象作为函数的返回值！", context.Exception);
            }
        }

        /// <summary>
        /// OnException: filter the BussinessException.
        /// </summary>
        /// <param name="context"></param>
        public void OnException(MethodContext context)
        {
            if (context == null) return;

            var bizException = context.Exception as BusinessException;
            if (bizException == null)
            {
                throw context.Exception;
            }
        }

        /// <summary>
        /// OnException: add the BussinessException to return value.
        /// </summary>
        /// <param name="context"></param>
        public void EndInvoke(MethodContext context)
        {
            if (context == null) return;            
            
            if(context.HasException)
            {
                var returnValue = new ResultEntity();
                returnValue.Fault = new FaultInfo
                {
                    Message = context.Exception.Message,
                    Type = FaultType.Info
                };
                context.ReturnValue = returnValue;
            }
        }

    }
}
