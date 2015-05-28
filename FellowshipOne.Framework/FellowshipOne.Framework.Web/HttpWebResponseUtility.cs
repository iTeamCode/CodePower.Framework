using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.Web
{

    public class HttpWebResponseUtility02
    {
        protected static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
            request.Timeout = timeout.HasValue ? timeout.Value : request.Timeout;
            
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
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

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
            request.Timeout = timeout.HasValue ? timeout.Value : request.Timeout;
             

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            byte[] postData = null;
            //如果需要POST数据  
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

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        protected static HttpWebResponse CreateHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            /*
             * 1.验证参数
             * 2.设置请求数据
             * 3.获取返回值
             */
            #region Check data.
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            #endregion Check data.

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

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
            request.Timeout = timeout.HasValue ? timeout.Value : request.Timeout;


            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            byte[] postData = null;
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                List<string> dataList = new List<string>();
                foreach (string key in parameters.Keys)
                {
                    dataList.Add(string.Format("{0}={1}", key, parameters[key]));
                }

                postData = requestEncoding.GetBytes(string.Join("&", dataList));
                request.ContentLength = postData.Length;
            }

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static void CreatePost(string url)
        {
            //Stream newStream = httpRequest.GetRequestStream();
            //httpRequest.Credentials (用户凭据信息)

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            //add request header information.
            #region add request header information.
            HttpRequestHeaders reqHeader = client.DefaultRequestHeaders;
            //reqHeader.Add("Content-Type", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            //reqHeader.Add("Content-Type", dataType);

            #endregion add request header information.
            HttpContent content = new StringContent(@"Test");


            //client.PostAsync(url, content);
            HttpResponseMessage response = client.PostAsync(new Uri(url), content).Result;
            String result = response.Content.ReadAsStringAsync().Result;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
    
}
