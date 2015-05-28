
namespace CodePower.Framework.Entitys
{

    /// <summary>
    /// 分页信息实体类
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// 获取或设置页面索引（第几页）
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 获取或设置每页显示数据量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取或设置总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 获取起始索引
        /// </summary>
        public int StartIndex
        {
            get
            {
                return PageIndex * PageSize;
            }
        }
    }
}

