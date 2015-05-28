using FellowshipOne.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FellowshipOne.Dashboard.XUnitTest
{
    public class WebApiTest
    {
        private readonly ITestOutputHelper _output;
        public WebApiTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        //[Fact]
        public void TestClient()
        {
            //string url = "http://localhost:53245/api/user";
            //HttpClient client = new HttpClient();

            //client.MaxResponseContentBufferSize = 256000;
            ////client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");

            //HttpResponseMessage response = client.GetAsync(new Uri(url)).Result;
            //String result = response.Content.ReadAsStringAsync().Result;
        }


        //[Fact]
        public void Framework_WebApi_SignIn()
        {
            string userName = "Alan";
            string password = "123";
            string urlSignIn = "http://localhost:53245/api/user/SignIn";
            

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Name", userName);
            parameters.Add("Password", password);

            string responseData;
            Encoding encoding = Encoding.UTF8;

            HttpWebResponse response = HttpWebResponseUtility02.CreatePostHttpResponse(urlSignIn, parameters, null, null, encoding, null);            
            System.IO.Stream respStream = response.GetResponseStream();
            
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, encoding))
            {
                responseData = reader.ReadToEnd();
            }
        }

        //[Fact]
        public void Framework_WebApi_SignIn_Get()
        {
            string urlSignIn = "http://localhost:53245/user/SignIn";
            
            Encoding encoding = Encoding.UTF8;

            string responseData;
            HttpWebResponse response = HttpWebResponseUtility02.CreateGetHttpResponse(urlSignIn, null, null);
            System.IO.Stream respStream = response.GetResponseStream();

            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, encoding))
            {
                responseData = reader.ReadToEnd();
            }

            string cookieString = response.Headers["Set-Cookie"]; 
        }
        
    }
}
