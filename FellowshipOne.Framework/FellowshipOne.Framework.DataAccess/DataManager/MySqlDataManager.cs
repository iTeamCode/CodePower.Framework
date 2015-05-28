using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using CodePower.Framework.Entitys;


namespace FellowshipOne.Framework.DataAccess
{
    public class MySqlDataManager : DataManager
    {

        protected override CustomerCommand CreateCommand(DataCommandConfig commandConfig)
        {
            //1.创建 Connection 对象【需要从连接池中获取连接对象，此处后续优化】
            string strConn = ConfigurationManager.AppSettings["ConnectionString"];
            
            MySqlConnection dbConnection = new MySqlConnection(strConn);

            //2.创建 Command 对象
            MySqlCommand command = new MySqlCommand(commandConfig.CommandText, dbConnection);
            command.CommandType = CommandType.Text;

            //3.填充参数列表
            foreach (ParameterConfig param in commandConfig.Parameters)
            {
                MySqlParameter parameter = command.CreateParameter();
                parameter.ParameterName = param.Name;
                parameter.DbType = param.DBType;
                parameter.Size = param.Size == 0 ? 4 : param.Size;
                parameter.Direction = ParameterDirection.Input; //默认是输入参数
                command.Parameters.Add(parameter);
            }
            CustomerCommand customerCmd = new CustomerCommand(command, DataBaseType.MySQL);
            return customerCmd;
        }
    }
}
