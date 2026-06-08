using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phòng ban không được để trống")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Lương cơ bản không được để trống")]
        [Range(1000000, 500000000, ErrorMessage = "Lương cơ bản phải từ 1 đến 500 triệu VNĐ")]
        public decimal BaseSalary { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái làm việc")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
    }
}