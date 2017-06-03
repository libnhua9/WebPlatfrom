namespace Web.Models.WebModels
{
    /// <summary>
    /// 查询列表的参数
    /// </summary>
    public class SearchParam<T,T2>
    {
        public T X { get; set; }

        public T2 Y { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 第几页
        /// </summary>
        public int Offset { get; set; }
    }
}