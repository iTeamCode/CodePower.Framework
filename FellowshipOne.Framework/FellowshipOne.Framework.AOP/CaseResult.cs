using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    public class ResultEntity
    {
        public virtual FaultInfo Fault { get; set; }
    }

    public sealed class VoidResultEntity : ResultEntity
    {
        public override FaultInfo Fault { get; set; }
    }

    public sealed class ResultEntity<T> : ResultEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public T Entity { get; set; }

        public override FaultInfo Fault { get; set; }
    }

    public class FaultInfo
    {
        public string Message { get; set; }
        public FaultType Type { get; set; }
    }

    public enum FaultType
    {
        Info,
        Warning,
        Error
    }
}
