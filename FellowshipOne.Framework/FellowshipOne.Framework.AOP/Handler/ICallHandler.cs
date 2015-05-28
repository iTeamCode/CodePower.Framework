using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    /// <summary>
    /// 调用句柄接口（定义客户端代码可以操作的方法）
    /// </summary>
    public interface ICallHandler
    {
        /// <summary>
        /// before the begin function Invoked （调用前的处理）
        /// </summary>
        /// <param name="context"></param>
        void BeginInvoke(MethodContext context);

        /// <summary>
        /// after the end function Invoked （调用前的处理）
        /// </summary>
        /// <param name="context"></param>
        void EndInvoke(MethodContext context);

        /// <summary>
        /// On Exception（异常处理）
        /// </summary>
        /// <param name="context"></param>
        void OnException(MethodContext context);
    }
}
