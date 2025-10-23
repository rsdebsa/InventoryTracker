using InventoryTracker.Domain.Entities;
using InventoryTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.API.Controllers
{
    /// <summary>
    /// API مدیریت محصولات: CRUD کامل با EF Core.
    /// Route پایه: /api/products
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        /// <summary>DbContext از DI تزریق می‌شود.</summary>
        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// دریافت لیست همه‌ی محصولات.
        /// GET: /api/products
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var items = await _db.Products
                                 .OrderBy(p => p.Name)
                                 .ToListAsync();
            return Ok(items);
        }

        /// <summary>
        /// دریافت یک محصول با Id.
        /// GET: /api/products/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return NotFound(); // 404 اگر پیدا نشد
            return Ok(product);
        }

        /// <summary>
        /// ایجاد محصول جدید.
        /// نکته: SKU باید یکتا باشد (ایندکس یکتا در DB).
        /// POST: /api/products
        /// Body(JSON): { "name":"...", "sku":"...", "price":0, "quantity":0 }
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product model)
        {
            // اعتبارسنجی ساده سمت سرور
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.SKU))
                return BadRequest("Name and SKU are required.");

            // افزودن و ذخیره
            _db.Products.Add(model);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // اگر علت خطا یونیک بودن SKU باشد، پیام مناسب برگردان
                if (ex.InnerException?.Message.Contains("IX_Products_SKU") == true ||
                    ex.Message.Contains("IX_Products_SKU"))
                {
                    return Conflict("SKU must be unique.");
                }
                throw; // بقیه‌ی خطاها را به بالاتر بده
            }

            // 201 Created + آدرس منبع
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        /// <summary>
        /// ویرایش یک محصول موجود.
        /// PUT: /api/products/{id}
        /// Body(JSON): { "id":1, "name":"...", "sku":"...", "price":0, "quantity":0 }
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product model)
        {
            if (id != model.Id)
                return BadRequest("Route id and body id must match.");

            // Track موجود را از DB بگیر
            var entity = await _db.Products.FindAsync(id);
            if (entity is null) return NotFound();

            // فیلدها را به‌روزرسانی کن
            entity.Name = model.Name;
            entity.SKU = model.SKU;
            entity.Price = model.Price;
            entity.Quantity = model.Quantity;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Products_SKU") == true ||
                    ex.Message.Contains("IX_Products_SKU"))
                {
                    return Conflict("SKU must be unique.");
                }
                throw;
            }

            return NoContent(); // 204
        }

        /// <summary>
        /// حذف محصول.
        /// DELETE: /api/products/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Products.FindAsync(id);
            if (entity is null) return NotFound();

            _db.Products.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
