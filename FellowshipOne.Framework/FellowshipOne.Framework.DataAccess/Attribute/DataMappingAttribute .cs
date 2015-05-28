using System;
using System.Data;

namespace FellowshipOne.Framework.DataAccess
{
    /// <summary>
    /// 实体关系映射[ORM]标签属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataMappingAttribute : Attribute 
    {
        /// <summary>
        /// 关联类型名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// 实体关系映射[ORM]构造函数
        /// </summary>
        /// <param name="columnName">关联类型名称</param>
        /// <param name="type"> 数据类型</param>
        public DataMappingAttribute(string columnName, DbType type)
        {
            this.ColumnName = columnName;
            this.Type = type;
        }
    }
}
