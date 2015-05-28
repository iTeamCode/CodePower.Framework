using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    public class MethodContext
    {
        /// <summary>
        /// 获取或设置执行者
        /// </summary>
        public object Executor { get; set; }

        /// <summary>
        /// 获取或设置类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 获取或设置方法名称
        /// </summary>
        public string MethodName { get; set; }
        
        /// <summary>
        /// 获取或设置返回类型名称
        /// </summary>
        public Type ReturnType { get; set; }

        /// <summary>
        /// 获取或设置返回值
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// 获取或设置是否终止调用
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// 获取或设置参数列表
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// 异常数据
        /// </summary>
        public Exception Exception { get; set; }

        public bool HasException
        {
            get { return this.Exception != null; }
        }
    }
}
