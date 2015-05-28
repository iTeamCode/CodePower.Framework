using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.Web.ApiClient
{
    /// <summary>
    /// A class manager all web api which use in system.
    /// </summary>
    public class ApiManager
    {
        #region Singleton Pattern
        private static ApiManager _instance;

        public static ApiManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ApiManager();
                }
                return _instance;
            }
        }

        private ApiManager()
        {
            LoadApiService();
        }

        #endregion Singleton Pattern

        protected Dictionary<string, ApiService> registerService { get; set; }

        protected void LoadApiService()
        {
            this.registerService = new Dictionary<string, ApiService>();
            //load config.
            ApiService apiService = new ApiService();
            apiService.Key = "Dashboard-User-SignIn";
            apiService.Method = RequestMethod.Post;
            apiService.BasicUrl = "http://localhost:53245";
            apiService.Url = "api/user/SignIn";

            this.registerService.Add(apiService.Key, apiService);
        }

        public ApiService GetRegisterServic(string key)
        {
            if (!this.registerService.ContainsKey(key))
            {
                throw new KeyNotFoundException(string.Format("Can't find the api service whick key is '{0}'", key));
            }
            return this.registerService[key];
        }

        public ApiService this[string key]
        {
            get {
                return this.registerService[key];
            }
        }

        
    }
}
