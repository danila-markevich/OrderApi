using System.ComponentModel.DataAnnotations;

namespace OrderApi.DTOs
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [Range(0.01, 1000000, ErrorMessage = "Цена должна быть от 0.01 до 1000000")]
        public decimal Price { get; set; }
    }
}