using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.Web.ApiClient
{

    public enum RequestMethod
    {
        Get,
        Post,
        Put,
        Delete
    }

    public class ApiService
    {
        public string Key { get; set; }

        public RequestMethod Method { get; set; }

        public string Url { get; set; }

        public string BasicUrl { get; set; }
    }
}
