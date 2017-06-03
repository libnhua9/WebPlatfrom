using System.Web.UI.WebControls;

namespace Web.Models.WebModels
{
    public class PageSearchResult<T>
    {
        public PageSearchResult(int? total,T data)
        {
            Total = total ?? 0;
            Data = data;
        }

        public int Total { get; private set; }

        public T Data { get; private set; }
    }
}