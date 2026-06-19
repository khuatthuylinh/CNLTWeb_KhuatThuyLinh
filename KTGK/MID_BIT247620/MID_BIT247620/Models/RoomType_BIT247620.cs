using System.ComponentModel.DataAnnotations;

namespace MID_BIT247620.Models
{
    public class RoomType_BIT247620
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại phòng không được để trống")]
        [Display(Name = "Tên loại phòng")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public virtual ICollection<Room_BIT247620> Rooms { get; set; } = new List<Room_BIT247620>();
    }
}
