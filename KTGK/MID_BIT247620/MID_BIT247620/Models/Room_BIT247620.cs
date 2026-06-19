using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MID_BIT247620.Models
{
    public class Room_BIT247620
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [Display(Name = "Tên phòng")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá phòng không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phòng phải lớn hơn 0")]
        [Display(Name = "Giá phòng (VND)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Diện tích không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0")]
        [Display(Name = "Diện tích (m²)")]
        public double Area { get; set; }

        [Display(Name = "Còn phòng")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
        [Display(Name = "Loại phòng")]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        [Display(Name = "Loại phòng")]
        public virtual RoomType_BIT247620? RoomType { get; set; }

        public virtual ICollection<RoomImage_BIT247620> RoomImages { get; set; } = new List<RoomImage_BIT247620>();

        [NotMapped]
        [Display(Name = "Giá/m² (VND)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal PricePerSquareMeter
        {
            get
            {
                if (Area <= 0) return 0;
                return Price / (decimal)Area;
            }
        }
    }
}
