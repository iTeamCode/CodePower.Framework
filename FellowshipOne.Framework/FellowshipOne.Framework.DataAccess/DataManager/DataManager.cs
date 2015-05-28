using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using CodePower.Framework.Entitys;

namespace FellowshipOne.Framework.DataAccess
{
    public abstract class DataManager
    {
        #region 操作方法
        /// <summary>
        /// 创建可用的Command对象
        /// </summary>
        /// <param name="commandName">需要获取的Command名称</param>
        /// <returns></returns>
        public CustomerCommand CreateCustomerCommand(string commandName)
        {
            /*
             * 1.获取 XML 数据
             * 2.获取对应的 Commmand 信息
             * 3.填充 Command 对象并返回数据
             */
            #region 1.获取 XML 数据
            //1.获取 XML 数据【可修改问配置文件】
            //string commandFilePath = @"F:\项目文档\SourceCode\ShAgricultureSaleShop\ShAgricultureSaleShop.WebUI\Configs\Data\DataCommand.xml";
            string commandFilePath = ConfigurationManager.AppSettings["CommandFilePath"];
            string strNamespace = @"https:\\North\Framework\DataOperators";

            FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + commandFilePath);
            XmlSerializer xmls = new XmlSerializer(typeof(DataOperatorsConfig), strNamespace);
            DataOperatorsConfig DataOperators = xmls.Deserialize(fs) as DataOperatorsConfig;
            fs.Close();
            #endregion

            #region 2.获取对应的 Commmand 信息
            //2.获取对应的 Commmand 信息
            DataCommandConfig commandConfig = DataOperators.DataCommands.Find(x => x.Name == commandName);
            /*后续添加针对 commandConfig 对象的数据校验*/
            if (commandConfig == null)
            {
                throw new Exception(string.Format("未找到Name='{0}'的配置节点，请检查配置文件是否正确。", commandName));
            }
            #endregion
            
            //3.填充 Command 对象
            CustomerCommand customerCmd = CreateCommand(commandConfig);
            return customerCmd;
        }
        #endregion 操作方法


        /// <summary>
        /// 创建命令对象
        /// </summary>
        /// <param name="commandConfig">命令对象配置文件</param>
        /// <returns>命令对象</returns>
        protected abstract CustomerCommand CreateCommand(DataCommandConfig commandConfig);

    }
}
