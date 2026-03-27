using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "The order must contain at least one product.")]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
