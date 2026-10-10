using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.DTOs;
using OrderApi.Models;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var orders = await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalSum = o.Items.Sum(i => i.Quantity * i.Product.Price),
                    ItemsCount = o.Items.Sum(i => i.Quantity)
                })
                .ToListAsync();

            return orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalSum = o.Items.Sum(i => i.Quantity * i.Product.Price),
                    ItemsCount = o.Items.Sum(i => i.Quantity)
                })
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create (CreateOrderDto dto)
        {
            var order = new Order
            {
                CustomerName = dto.CustomerName,
                OrderDate = DateTime.Now,
                Status = dto.Status,
                Items = new List<OrderItem>()
            };

            decimal totalSum = 0;
            int itemsCount = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with id {itemDto.ProductId} not found");
                }

                order.Items.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity
                });

                totalSum += itemDto.Quantity * product.Price;  
                itemsCount += itemDto.Quantity;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var result = new OrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalSum = totalSum,
                ItemsCount = itemsCount
            };

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateOrderDto dto)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null) return NotFound();
            existing.CustomerName = dto.CustomerName;
            existing.Status = dto.Status;

            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<List<OrderItemDto>>> GetItems (int id)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == id);
            if (!orderExists) return NotFound("Order not found");

            var items = await _context.OrderItems
                .Where (oi => oi.OrderId == id)
                .Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product != null ? oi.Product.Name : null,
                    Price = oi.Product != null ? oi.Product.Price : 0,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.Quantity * (oi.Product != null ? oi.Product.Price : 0)
                })
                .ToListAsync();
            return items;
           
        }
    }
}
