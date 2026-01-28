namespace ApiVersioning.Pagination
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);

        public List<T> Data { get; set; }
    }

}
