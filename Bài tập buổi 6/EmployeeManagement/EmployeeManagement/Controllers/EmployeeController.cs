using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // Danh sách 
        private static List<Employee> _employeeList = new List<Employee>()
        {
            new Employee { Id = 1, FullName = "Khuất Thùy Linh", Department = "Kỹ thuật", BaseSalary = 50000000, Status = "Đang làm việc", Email = "nanobanana@khuat.com" },
            new Employee { Id = 2, FullName = "Khuất Quang Tuấn", Department = "Nhân sự", BaseSalary = 15000000, Status = "Đang làm việc", Email = "tubaboda@khuat.com" }
        };

        // 1. Hiển thị và Tìm kiếm 
        public IActionResult Index(string searchString)
        {
            var employees = from e in _employeeList select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                              || s.Department.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                ViewData["CurrentFilter"] = searchString;
            }
            return View(employees.ToList());
        }

        // 2. Chức năng Xem Chi Tiết (Detail)
        public IActionResult Detail(int id)
        {
            var employee = _employeeList.FirstOrDefault(x => x.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // 3. Chức năng Thêm mới (Create - GET: Hiển thị Form)
        public IActionResult Create()
        {
            return View();
        }

        // 3. Chức năng Thêm mới (Create - POST: Nhận dữ liệu từ Form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _employeeList.Count > 0 ? _employeeList.Max(x => x.Id) + 1 : 1;
                _employeeList.Add(model);
                TempData["SuccessMessage"] = "Thêm nhân viên mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 4. Chức năng Chỉnh sửa (Edit - GET: Lấy dữ liệu cũ đổ lên Form)
        public IActionResult Edit(int id)
        {
            var employee = _employeeList.FirstOrDefault(x => x.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // 4. Chức năng Chỉnh sửa (Edit - POST: Cập nhật dữ liệu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeList.FirstOrDefault(x => x.Id == model.Id);
                if (employee != null)
                {
                    employee.FullName = model.FullName;
                    employee.Department = model.Department;
                    employee.BaseSalary = model.BaseSalary;
                    employee.Status = model.Status;
                    employee.Email = model.Email;
                    TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(model);
        }

        // 5. Chức năng Xóa (Delete - GET: Trang Cảnh báo)
        public IActionResult Delete(int id)
        {
            var employee = _employeeList.FirstOrDefault(x => x.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // 5. Chức năng Xóa (Delete - POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _employeeList.FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
                TempData["SuccessMessage"] = "Đã xóa nhân viên khỏi hệ thống!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}