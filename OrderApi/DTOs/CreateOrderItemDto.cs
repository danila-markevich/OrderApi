using System.ComponentModel.DataAnnotations;

namespace OrderApi.DTOs
{
    public class CreateOrderItemDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "ProductId обязателен")]
        public int ProductId { get; set; }

        [Range(1, 1000, ErrorMessage = "Количество от 1 до 1000")]
        public int Quantity { get; set; }
    }
}