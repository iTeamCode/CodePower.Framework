using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    /// <summary>
    /// Cache Process
    /// </summary>
    public class CacheCallHandler : ICallHandler
    {
        /// <summary>
        /// Cache default duration minutes.
        /// </summary>
        private const int DefaultDurationMinutes = 30;

        /// <summary>
        /// Cache Key.
        /// </summary>
        private string _cacheKey;
        /// <summary>
        /// duration minutes.
        /// </summary>
        private int _durationMinutes;

        /// <summary>
        /// Constructor Function
        /// </summary>
        /// <param name="attributes">attribute collection. </param>
        public CacheCallHandler(NameValueCollection attributes)
        {
            _cacheKey = String.IsNullOrEmpty(attributes["CacheKey"]) ? string.Empty : attributes["CacheKey"];
            _durationMinutes = String.IsNullOrEmpty(attributes["DurationMinutes"]) ? DefaultDurationMinutes : int.Parse(attributes["DurationMinutes"]);
        }
        public CacheCallHandler()
        { 

        }
        public void BeginInvoke(MethodContext context)
        {
            //context.ReturnValue = "Cache Result";
            //context.Processed = true;
            //
        }

        public void EndInvoke(MethodContext context)
        {

        }

        public void OnException(MethodContext context)
        {
            //Console.WriteLine(context.Exception.Message);
        }
       
    }
}
