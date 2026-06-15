using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name là bắt buộc")]
        [MinLength(3, ErrorMessage = "Name phải có ít nhất 3 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue,
            ErrorMessage = "Price phải lớn hơn 0")]
        public decimal Price { get; set; }
    }
}