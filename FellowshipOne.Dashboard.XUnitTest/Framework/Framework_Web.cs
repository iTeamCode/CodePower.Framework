using FellowshipOne.Framework.Web.ApiClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dashboard.XUnitTest.Framework
{
    public class Framework_Web
    {
        [Fact]
        public void RequestMaker_Get()
        {
           

            //ApiService apiService = new ApiService();
            //apiService.Key = "Key-UnitTestApi";
            //apiService.Method = RequestMethod.Post;
            //apiService.BasicUrl = "http://localhost:53245";
            //apiService.Url = "api/user/SignIn";

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Name", "Alan");
            parameters.Add("Password", "123");

            //var rm = new RequestMaker(apiService);
            var rm = new RequestMaker(ApiManager.Instance["Dashboard-User-SignIn"]);
            var response = rm.SendRequest(parameters, null);
            
            Encoding encoding = Encoding.UTF8;
            Stream respStream = response.GetResponseStream();
            string responseData;
            using (StreamReader reader = new StreamReader(respStream, encoding))
            {
                responseData = reader.ReadToEnd();
            }
            //Assert.Equal(responseData, "{}");

            //httpWebResponse.GetResponseStream().
            //if (rd.Error != null) { };
            //string msg = rd.OriginalMessage;
            //var model msg = rd.ToEntity<User>();

            //rm.SendRequest(data);
            //rm.SetData();
        }

        public void RequestMaker_Post()
        { 

        }
    }
}
