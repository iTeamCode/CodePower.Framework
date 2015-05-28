using FellowshipOne.Framework.DataAccess;
using CodePower.Framework.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Dashboard.DataAccess
{
    public class User
    {
        [DataMapping("ID", System.Data.DbType.Int16)]
        public int ID { get; set; }
    }
    public class UserDataManager
    {
        private static DataManager _dataManager = DataManagerFactory.CreateDataManager(DataBaseType.SQLServer);

        public static bool AuthenticateUser(int churchId, string name, string pwd)
        {
            bool isPass = false;
            CustomerCommand command = _dataManager.CreateCustomerCommand("Dashboard.AuthenticateUser");
            command.SetParameterValue("@ChurchID", churchId);
            command.SetParameterValue("@Name", name);
            command.SetParameterValue("@Password", pwd);


            User user = command.ExecuteCommandToEntity<User>();
            try
            {
                isPass = Convert.ToBoolean(command.ExecuteCommandScalar());
            }
            catch (Exception ex)
            {
                //Log here.
            }

            return isPass;
        }
    }
}
