using FellowshipOne.Dashboard.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Dashboard.ApiBizLogic
{
    public class UserBizManager
    {
        public static bool AuthenticateUser(int churchId, string name, string pwd)
        {
            //some logic code here.
            return UserDataManager.AuthenticateUser(churchId, name, pwd);
        }
    }
}
