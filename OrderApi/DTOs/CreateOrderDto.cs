using System.ComponentModel.DataAnnotations;

namespace OrderApi.DTOs
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Имя клиента обязательно")]
        [StringLength(100, MinimumLength = 1)]
        public string CustomerName { get; set; } = "";

        [StringLength(50)]
        public string Status { get; set; } = "";

        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}