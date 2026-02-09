namespace ApiVersioning.Dtos
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
