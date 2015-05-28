using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.Web.ApiClient
{
    public class RequestMaker
    {
        protected ApiService _apiService;

        public RequestMaker(ApiService apiService)
        {
            _apiService = apiService;
        }

        public HttpWebResponse SendRequest(IDictionary<string, string> parameters, CookieCollection cookies)
        {
            if (_apiService == null)
            {
                throw new ArgumentNullException("_apiService");
            }

            string url = string.Format("{0}/{1}", _apiService.BasicUrl, _apiService.Url);
            string method = _apiService.Method.ToString();

            HttpWebResponse httpWebResponse = CreateHttpResponse(url, method, parameters, null, null, Encoding.UTF8, cookies);
            return httpWebResponse;
        }

        public HttpWebResponse SendRequest() { throw new NotImplementedException(); }

        public HttpWebResponse SendRequest<T>(T entity) where T : new()
        {
            throw new NotImplementedException();
        }

        public HttpWebResponse SendRequest(string json)
        {

            throw new NotImplementedException();
        }

        #region create request
        protected readonly string _defaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        /// <summary>  
        /// 创建HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>
        /// <param name="method">请求的method</param>
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public HttpWebResponse CreateHttpResponse(string url, string method, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            /*
             * 1.验证参数
             * 2.设置请求数据
             * 3.获取返回值
             */

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            //@.Set request infomation.
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.CreateHttp(url);
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.CreateHttp(url);
            }

            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = string.IsNullOrEmpty(userAgent) ? _defaultUserAgent : userAgent;
            request.Timeout = timeout.HasValue ? timeout.Value : request.Timeout;


            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            
            //@.Fill post data.
            byte[] postData = null;
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                List<string> dataList = new List<string>();


                foreach (string key in parameters.Keys)
                {
                    dataList.Add(string.Format("{0}={1}", key, parameters[key]));
                }

                postData = requestEncoding.GetBytes(string.Join("&", dataList));
                request.ContentLength = postData.Length;

            }

            if (postData != null && method.ToUpper() != "GET")
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        #endregion
    }
}
