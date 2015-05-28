using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Dashboard.ApiService.Controllers
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class UserController : ApiController
    {

        //[HttpPost]
        //public User SignOut(string userName, string password)
        //{
        //    //string userName = "userName";
        //    //string password = "password";
        //    //string data = string.Format("['UserName':'{0}','Password','{1}']", userName, password);
        //    //return Json(data);

        //    return new User() { ID = 1, Name = userName, Password = password };
        //}

        [HttpPost]
        public IList<User> SignIn([FromBody]User user)
        {
            user.ID = 1;
            if (user != null)
            {
                string data = string.Format("['UserName':'{0}','Password','{1}']", user.Name, user.Password);
            }
            user.CreateDate = DateTime.Now;
            var users = new List<User>();
            users.Add(user);
            users.Add(new User() { ID = 2, Name = "Lucas", Password = "888", CreateDate = DateTime.Now.AddDays(10) });
            return users;
        }

        [HttpPost]
        public bool SignOut([FromBody]int id)
        {
            throw new NotImplementedException();
        }
	}
}