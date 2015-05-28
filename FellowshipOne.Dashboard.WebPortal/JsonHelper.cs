using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace FellowshipOne.Dashboard.WebPortal
{
    public class JsonHelper
    {
        public static T Deserialize<T>(string data)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Deserialize<T>(data);
        }

        /// <summary>
        /// 把对象转成json字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string Serialize(object o)
        {
            StringBuilder sb = new StringBuilder();
            JavaScriptSerializer json = new JavaScriptSerializer();
            json.Serialize(o, sb);
            return sb.ToString();
        }
    }
}