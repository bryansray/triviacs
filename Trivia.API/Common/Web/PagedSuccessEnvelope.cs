namespace Trivia.API.Common.Web
{
    public class PagedSuccessEnvelope : SuccessEnvelope
    {
        public PaginationMeta Meta { get; }

        public PagedSuccessEnvelope(object data, int page, int pageSize, int total) : base(data)
        {
            Meta = new PaginationMeta(page, pageSize, total);
        }
    }
    
    public class PaginationMeta
    {
        public PaginationMeta(int page, int pageSize, int total)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
        }

        public int Page { get; }
        
        public int PageSize { get; }
        
        public int Total { get; }
        
        public int TotalPages { get; set; }
    }
}