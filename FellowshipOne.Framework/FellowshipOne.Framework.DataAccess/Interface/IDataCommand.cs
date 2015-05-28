using System.Collections.Generic;
using System.Data;

namespace FellowshipOne.Framework.DataAccess
{
    /// <summary>    
    /// 数据命令对象接口
    /// </summary>    
    public interface IDataCommand
    {

        #region 固有属性
        
        #endregion 固有属性

        #region 可执行方法
        /// <summary>
        /// 执行命令并将结果填充到DataSet中        
        /// </summary>
        /// <returns>填充后的实体类</returns>
        DataSet ExecuteCommandToDataSet();

        /// <summary>
        /// 执行命令并将结果填充到实体类
        /// </summary>
        /// <typeparam name="T">需要填充的实体类类型</typeparam>
        /// <returns>填充后的实体类</returns>
        T ExecuteCommandToEntity<T>() where T : class,new();

        /// <summary>
        /// 执行命令并将结果填充到实体类List
        /// </summary>
        /// <typeparam name="T">需要填充的实体类类型</typeparam>
        /// <returns>填充后的实体类List</returns>
        List<T> ExecuteCommandToEntitys<T>() where T : new();

        #endregion
    }
}

