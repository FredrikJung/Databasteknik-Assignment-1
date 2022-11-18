using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagerApp.WebApi.Contexts;
using OrderManagerApp.WebApi.Models.Entities;
using OrderManagerApp.WebApi.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OrderManagerApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(OrderRequest orderRequest)
        {
            try
            {
                var orderEntity = new OrderEntity
                {
                    OrderDate = orderRequest.OrderDate = DateTime.Now,
                    DueDate = orderRequest.DueDate = DateTime.Now.AddDays(30),
                    TotalPrice = orderRequest.TotalPrice,
                    CustomerId = orderRequest.CustomerId
                     
                };
                _context.Orders.Add(orderEntity);
                await _context.SaveChangesAsync();

                foreach (var product in orderRequest.Products)
                {
                    var orderRowEntity = new OrderRowEntity
                    {
                        OrderId = orderEntity.OrderId,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        UnitPrice = product.Price
                    };

                    _context.OrdersRows.Add(orderRowEntity);
                    await _context.SaveChangesAsync();
                }
                
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
