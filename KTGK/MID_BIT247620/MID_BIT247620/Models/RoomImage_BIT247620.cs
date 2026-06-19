using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MID_BIT247620.Models
{
    public class RoomImage_BIT247620
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh không được để trống")]
        [Display(Name = "Đường dẫn ảnh (URL)")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Ảnh đại diện")]
        public bool IsThumbnail { get; set; } = false;

        [Required(ErrorMessage = "Phòng không được để trống")]
        [Display(Name = "Phòng")]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room_BIT247620? Room { get; set; }
    }
}
