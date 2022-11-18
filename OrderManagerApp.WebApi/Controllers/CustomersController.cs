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
    public class CustomersController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = new List<CustomerModel>();

            try
            {
                foreach (var type in await _context.Customers.ToListAsync())
                    customers.Add(new CustomerModel
                    {
                        CustomerId = type.CustomerId,
                        Name = type.Name
                    });
                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return customers;

        }

        [HttpGet("id")]
        public async Task<ActionResult<CustomerModel>> GetAsync(int id)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);

                if (customerEntity == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(new CustomerModel
                {
                    CustomerId = customerEntity.CustomerId,
                    Name = customerEntity.Name
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();

        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id, CustomerRequest customerRequest)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);
                if (customerEntity != null)
                {
                    customerEntity.Name = customerRequest.Name;
                    _context.Entry(customerEntity).State = EntityState.Modified;
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
        public async Task<ActionResult> CreateAsync(CustomerRequest req)
        {
            try
            {
                var customerEntity = new CustomerEntity()
                {
                    Name = req.Name
                };
                _context.Customers.Add(customerEntity);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new CustomerModel
                {
                    CustomerId = customerEntity.CustomerId,
                    Name = customerEntity.Name
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpDelete("id")]
        public async Task<ActionResult<CustomerModel>> DeleteAsync(int id)
        {
            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);
                if (customerEntity == null)
                {
                    return new NotFoundResult();
                }
                _context.Customers.Remove(customerEntity);
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
