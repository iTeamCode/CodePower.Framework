
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using CodePower.Framework.Entitys;

namespace FellowshipOne.Framework.DataAccess
{
    /// <summary>
    /// 数据库访问自定义Command对象
    /// </summary>
    public class CustomerCommand : IDataCommand
    {
        
        internal CustomerCommand(DbCommand command, DataBaseType databaseType)
        {
            this._command = command;
            this._databaseType = databaseType;
        }

        #region 私有字段
        /// <summary>
        /// 数据命令对象
        /// </summary>
        private DbCommand _command;
        /// <summary>
        /// 数据库类型
        /// </summary>
        private DataBaseType _databaseType;

        #endregion 

        #region 公开属性
        /// <summary>
        /// 获取或设置CommandText
        /// </summary>
        public string CommandText
        {
            get
            {
                string strCommandText = string.Empty;
                if (this._command != null)
                {
                    strCommandText = this._command.CommandText;
                }
                return strCommandText;
            }
            set
            {
                if (this._command != null)
                {
                    this._command.CommandText = value;
                }
            }
        }
        #endregion

        #region 可执行方法[抽象基类方法]

        #region 操作相关
        /// <summary>
        /// 执行命令并将结果填充到DataSet中        
        /// </summary>
        /// <returns>填充后的实体类</returns>
        public DataSet ExecuteCommandToDataSet()
        {
            DataSet dsResult = new DataSet();

            if (_command == null)
            {
                throw new Exception();
            }
            
            switch (this._databaseType)
            {
                case DataBaseType.SQLServer:
                    SqlDataAdapter adapterForSQL = new SqlDataAdapter((SqlCommand)_command);
                    adapterForSQL.Fill(dsResult);
                    break;
                case DataBaseType.Oracle:
                case DataBaseType.DB2:
                    throw new Exception(string.Format("未处理的数据库类型{0}", _databaseType));
                    //break;
                case DataBaseType.MySQL:
                    MySqlDataAdapter adapterForMySql = new MySqlDataAdapter((MySqlCommand)_command);
                    adapterForMySql.Fill(dsResult);
                    break;
                default:
                    throw new Exception(string.Format("未处理的数据库类型{0}", _databaseType));
                    //break;
            }
            return dsResult;
        }

        /// <summary>
        /// 执行命令并将结果填充到实体类
        /// </summary>
        /// <typeparam name="T">需要填充的实体类类型</typeparam>
        /// <returns>填充后的实体类</returns>
        public T ExecuteCommandToEntity<T>() where T : class,new()
        {
            if (_command == null)
            {
                throw new Exception();
            }

            Type type = typeof(T);
            Dictionary<string, PropertyInfo> dicFields = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo field in type.GetProperties())
            {
                DataMappingAttribute attr = field.GetCustomAttribute(typeof(DataMappingAttribute)) as DataMappingAttribute;
                if (attr != null)
                {
                    dicFields.Add(attr.ColumnName, field);
                }
            }
            
            //从DB获取数据
            T entity = null;
            _command.Connection.Open();
            DbDataReader dr = _command.ExecuteReader();
            if (dr.Read())
            {
                entity = new T();
                string fileName = string.Empty;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    fileName = dr.GetName(i);
                    if (dicFields.Keys.Contains(fileName) && dr[i] != DBNull.Value)
                    {
                        dicFields[dr.GetName(i)].SetValue(entity, dr[i]);
                    }
                }
            }
            _command.Connection.Close();

            return entity;
        }

        /// <summary>
        /// 执行命令并将结果填充到实体类List
        /// </summary>
        /// <typeparam name="T">需要填充的实体类类型</typeparam>
        /// <returns>填充后的实体类List</returns>
        public List<T> ExecuteCommandToEntitys<T>() where T : new()
        {
            if (_command == null)
            {
                throw new Exception();
            }

            Type type = typeof(T);
            List<T> entityList = new List<T>();
            Dictionary<string, PropertyInfo> dicFields = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo field in type.GetProperties())
            {
                DataMappingAttribute attr = field.GetCustomAttribute<DataMappingAttribute>();
                if (attr != null)
                {
                    dicFields.Add(attr.ColumnName, field);
                }
            }

            //从DB获取数据
            _command.Connection.Open();
            DbDataReader dr = _command.ExecuteReader();
            string fileName = string.Empty;

            while (dr.Read())
            {
                T entity = new T();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    fileName = dr.GetName(i);
                    if (dicFields.Keys.Contains(fileName) && dr[i] != DBNull.Value)
                    {
                        dicFields[dr.GetName(i)].SetValue(entity, dr[i]);
                    }  
                }
                entityList.Add(entity);
            }
            dr.Close();
            _command.Connection.Close();

            return entityList;
        }

        /// <summary>
        /// 执行命令并将结果填充到实体类List
        /// </summary>
        /// <typeparam name="T">需要填充的实体类类型</typeparam>
        /// <param name="totalCount">返回总行数</param>
        /// <returns>填充后的实体类List</returns>
        public List<T> ExecuteCommandToEntitys<T>(out int totalCount) where T : new()
        {
            totalCount = 0;
            if (_command == null)
            {
                throw new Exception();
            }

            Type type = typeof(T);
            List<T> entityList = new List<T>();
            Dictionary<string, PropertyInfo> dicFields = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo field in type.GetProperties())
            {
                DataMappingAttribute attr = field.GetCustomAttribute<DataMappingAttribute>();
                if (attr != null)
                {
                    dicFields.Add(attr.ColumnName, field);
                }
            }

            //从DB获取数据
            _command.Connection.Open();
            DbDataReader dr = _command.ExecuteReader();
            string fileName = string.Empty;
            bool isQuery = true;
            do
            {
                if (isQuery == true)
                {
                    while (dr.Read())
                    {
                        T entity = new T();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            fileName = dr.GetName(i);
                            if (dicFields.Keys.Contains(fileName) && dr[i] != DBNull.Value)
                            {
                                dicFields[dr.GetName(i)].SetValue(entity, dr[i]);
                            }
                        }
                        entityList.Add(entity);
                    }
                    isQuery = false;
                }
                else
                {   //获取的二个查询里面的信息
                    if (dr.Read())
                    {
                        totalCount = Convert.ToInt32(dr[0]);
                    }
                }
            }
            while (dr.NextResult());
            dr.Close();
            _command.Connection.Close();

            return entityList;
        }

        /// <summary>
        /// 执行Commmand返回相应行数
        /// </summary>
        /// <returns>相应行数</returns>
        public int ExecuteCommandNonQuery()
        {
            if (_command == null)
            {
                throw new Exception();
            }

            //从DB获取数据
            _command.Connection.Open();
            int result = _command.ExecuteNonQuery();
            _command.Connection.Close();

            return result;
        }
        /// <summary>
        /// 执行Command 返回第一行第一列
        /// </summary>
        /// <returns>第一行第一列</returns>
        public object ExecuteCommandScalar()
        {
            if (_command == null)
            {
                throw new Exception();
            }

            //从DB获取数据
            _command.Connection.Open();
            object result = _command.ExecuteScalar();
            _command.Connection.Close();

            return result;
        }
        #endregion 操作相关

        #region 参数相关
        /// <summary>
        /// 设置已有的参数信息(Command.xml中配置的信息)
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        public void SetParameterValue(string parameterName, object value)
        {
            SetParameterValue(parameterName, value, ParameterDirection.Input);
        }
        /// <summary>
        /// 设置已有的参数信息(Command.xml中配置的信息)
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="paramDirection">输入\输出</param>
        public void SetParameterValue(string parameterName,object value,ParameterDirection paramDirection)
        {
            bool isSuccess = false; //是否操作成功
            if (this._command != null && this._command.Parameters != null)
            {
                foreach (DbParameter param in this._command.Parameters)
                {
                    if (param.ParameterName == parameterName)
                    { 
                        param.Value = value;
                        param.Direction = paramDirection;
                        isSuccess = true;
                    }
                }
            }
            if (isSuccess == false)
            {
                throw new Exception(string.Format("参数列表中不存在名称为{0}的参数，请见检查SQL配置文件的中的参数节点。", parameterName));
            }
        }
        /// <summary>
        /// 添加未配置输入参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        public void AddInputParameterValue(string parameterName, object value, DbType dbType)
        {
            AddParameterValue(parameterName, value, dbType, ParameterDirection.Input);
        }
        /// <summary>
        /// 添加未配置输出参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        public void AddOutputParameterValue(string parameterName, DbType dbType)
        {
            AddParameterValue(parameterName, null, dbType, ParameterDirection.Output);
        }
        /// <summary>
        /// 添加未配置参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// ><param name="paramDirection">输入\输出</param>
        /// <param name="dbType">参数类型</param>
        protected void AddParameterValue(string parameterName, object value,DbType dbType, ParameterDirection paramDirection)
        {
            if (this._command != null && this._command.Parameters != null)
            {
                //检查参数是否存在
                foreach (DbParameter param in this._command.Parameters)
                {
                    if (param.ParameterName == parameterName)
                    {
                        throw new Exception(string.Format("参数列表中已包含名称为{0}的参数！", parameterName));
                    }
                }

                //构建并添加新的参数实例
                DbParameter addParam = this._command.CreateParameter();
                addParam.ParameterName = parameterName;
                addParam.Value = value;
                addParam.DbType = dbType;
                addParam.Direction = paramDirection;
                this._command.Parameters.Add(addParam);
            }
            else
            {
                throw new Exception("未初始化的命令对象或参数列表！");
            }
        }

        /// <summary>
        /// 初始化分页信息
        /// </summary>
        /// <param name="paging">分页信息</param>
        public void InitPagingParameter(PagingInfo paging)
        {
            this.AddParameterValue("@StartIndex", paging.StartIndex, DbType.Int32, ParameterDirection.Input);
            this.AddParameterValue("@PageSize", paging.PageSize, DbType.Int32, ParameterDirection.Input);
            //this.AddParameterValue("@TotalCount", paging.TotalCount, DbType.Int32, ParameterDirection.Output);
        }
        #endregion 参数相关

        #endregion
    }
}
