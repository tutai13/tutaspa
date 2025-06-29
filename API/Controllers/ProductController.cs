using API.DTOs.Product;
using API.IService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO dto)
        {
            var result = await _productService.CreateAsync(dto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.SanPhamId }, result);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductCreateDTO dto)
        {
            var success = await _productService.UpdateAsync(id, dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        // GET: api/Product/name?ten=abc
        [HttpGet("name")]
        public async Task<IActionResult> SearchByName([FromQuery] string ten)
        {
            var result = await _productService.SearchByNameAsync(ten);
            return Ok(result);
        }
        // GET: api/Product/filter-by-price?min=1000&max=2000
        [HttpGet("filter-by-price")]
        public async Task<IActionResult> FilterByPrice([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var result = await _productService.FilterByPriceAsync(min, max);
            return Ok(result);
        }


    }
}
