using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagerApp.WebApi.Contexts;
using OrderManagerApp.WebApi.Models;
using OrderManagerApp.WebApi.Models.Entities;
using System.Diagnostics;

namespace OrderManagerApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = new List<ProductModel>();

            try
            {
                foreach (var type in await _context.Products.ToListAsync())
                    products.Add(new ProductModel
                    {
                        ProductId = type.ProductId,
                        Name = type.Name,
                        Price = type.Price
                    });

                return products;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return products;
        }

        [HttpGet("id")]
        public async Task<ProductModel> GetAsync(int id)
        {
            try
            {
                var productEntity = await _context.Products.FindAsync(id);
                if (productEntity == null)
                {
                    return new ProductModel();
                }

                return new ProductModel
                {
                    ProductId = productEntity.ProductId,
                    Name = productEntity.Name,
                    Price = productEntity.Price
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new ProductModel();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id, ProductRequest productRequest)
        {
            try
            {

                var productEntity = await _context.Products.FindAsync(id);
                if (productEntity != null)
                {
                    productEntity.Name = productRequest.Name;
                    productEntity.Price = productRequest.Price;
                    _context.Entry(productEntity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return new OkResult();
                }
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProductRequest req)
        {
            try
            {
                var productEntity = new ProductEntity()
                {
                    Name = req.Name,
                    Price = req.Price
                };

                _context.Products.Add(productEntity);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new ProductModel
                {
                    ProductId = productEntity.ProductId,
                    Name = productEntity.Name,
                    Price = productEntity.Price
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpDelete("id")]
        public async Task<ActionResult<ProductModel>> DeleteAsync(int id)
        {
            try
            {
                var productEntity = await _context.Products.FindAsync(id);

                if (productEntity == null)
                {
                    return new NotFoundResult();
                }

                _context.Products.Remove(productEntity);
                await _context.SaveChangesAsync();

                return new OkResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }
    }
}
