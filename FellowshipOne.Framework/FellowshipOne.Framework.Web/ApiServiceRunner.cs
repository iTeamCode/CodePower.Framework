using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.Web
{

    public enum RestfulType
    {
        Get,
        Post,
        Put,
        Delete
    }

    public class ResponseData
    {

    }

    public class ResponseData<T>
    {
        public T Data { get; set; }
    }


    public class ApiServiceRunner : IServiceRunner
    {
        public ResponseData RequestService() { return null; }
        public ResponseData RequestService(RestfulType restfulType) { return null; }
        public ResponseData RequestService<T>(T data, string dataType, RestfulType restfulType) { return null; }

        public ResponseData RequestService(string url, string data, RestfulType restfulType, string dataType = "")
        {
            HttpWebRequest httpRequest = WebRequest.CreateHttp(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.ContentLength = data.Length;

            httpRequest.GetResponse();
            


            return null;
        }

        public void TestFunction()
        {
            //Stream newStream = httpRequest.GetRequestStream();
            //httpRequest.Credentials (用户凭据信息)

            //HttpClient client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;

            ////add request header information.
            //#region add request header information.
            //HttpRequestHeaders reqHeader = client.DefaultRequestHeaders;
            ////reqHeader.Add("Content-Type", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            //reqHeader.Add("Content-Type", dataType);

            //#endregion add request header information.

            ////client.PostAsync(url,)

            //HttpResponseMessage response = client.GetAsync(new Uri(url)).Result;
            //String result = response.Content.ReadAsStringAsync().Result;
        }
    }
}
