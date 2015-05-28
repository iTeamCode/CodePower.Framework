using CodePower.Framework.Entitys;
using System;

namespace FellowshipOne.Framework.DataAccess
{
    public class DataManagerFactory
    {
        private DataManagerFactory() { }

        public static DataManager CreateDataManager(DataBaseType databaseType)
        {
            DataManager objDataManager;
            switch (databaseType)
            {
                case DataBaseType.SQLServer:
                    objDataManager = new SqlDataManager();
                    break;
                case DataBaseType.MySQL:
                    objDataManager = new MySqlDataManager();
                    break;
                case DataBaseType.Oracle:
                case DataBaseType.DB2:
                default:
                    throw new NotImplementedException();
                    //break;
            }

            return objDataManager;
        }
    }
}
