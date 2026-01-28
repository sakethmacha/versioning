namespace ApiVersioning.Dtos
{
    public class ProductFilterQuery
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Category { get; set; }
    }
}
