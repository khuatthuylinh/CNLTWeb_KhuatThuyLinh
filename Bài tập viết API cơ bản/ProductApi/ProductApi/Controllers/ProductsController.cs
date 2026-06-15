using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> products = new();

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(errors);
            }

            product.Id = products.Count + 1;

            products.Add(product);

            return Ok(product);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Id phải là số nguyên dương"
                });
            }

            var product = products
                .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm"
                });
            }

            return Ok(product);
        }
    }
}