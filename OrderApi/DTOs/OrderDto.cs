namespace OrderApi.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        public decimal TotalSum { get; set; }
        public int ItemsCount { get; set; }
    }
}
