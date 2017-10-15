using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise.DataBase
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class SelectParameter
    {
        /// <summary>
        /// 页面索引
        /// </summary>
        public int PageIndex;

        /// <summary>
        /// 页面数量
        /// </summary>
        public int PageCount;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderByValue;

        /// <summary>
        /// 排序方式(asc正序,desc倒序)
        /// </summary>
        public string OrderBy;

        public SelectParameter()
        {
            OrderByValue = "Id";
            OrderBy = "asc";
        }

        public SelectParameter(string orderbyValue,string orderBy)
        {
            this.OrderByValue = orderbyValue;
            this.OrderBy = orderBy;
        }

        public SelectParameter(string orderbyValue, string orderBy,int pageIndex,int pageCount)
        {
            this.OrderByValue = orderbyValue;
            this.OrderBy = orderBy;
            this.PageIndex = pageIndex;
            this.PageCount = pageCount;
        }

    }
}
