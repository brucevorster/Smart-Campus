namespace UmojaCampus.Shared.RequestFeatures
{
    public class QueryParameters
    {
        public string Filter { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
