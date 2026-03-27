using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class OrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "The amount must be greater than 0 and reasonable.")]
        public int Amount { get; set; }
    }
}
