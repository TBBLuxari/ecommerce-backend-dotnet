using Api.Data;
using Api.DTOs;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Api.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public OrdersController(AppDbContext context , IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

 
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) { return Unauthorized("No se pudo identificar al usuario."); }

            decimal totalAmount = 0;
            var orderDetails = new List<OrderDetail>();
            string filasFacturaHtml = "";

            var newOrder = new Order
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                Total = 0,
                Status = OrderStatus.Pendiente
            };

            foreach (var item in request.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null) { return BadRequest($"The product with ID {item.ProductId} does not exist."); }
                if (product.Stock < item.Amount) { return BadRequest("Insufficient stock"); }

                product.Stock -= item.Amount;
                decimal subTotal = product.Price * item.Amount;
                totalAmount += subTotal;

                var detail = new OrderDetail
                {
                    OrderId = newOrder.Id,
                    ProductId = product.Id,
                    Amount = item.Amount,
                    SinglePrice = product.Price,
                };

                orderDetails.Add(detail);          
                filasFacturaHtml += $@"
                    <tr>
                        <td style='padding: 8px; border-bottom: 1px solid #ddd;'>{product.Name}</td>
                        <td style='padding: 8px; border-bottom: 1px solid #ddd; text-align: center;'>{item.Amount}</td>
                        <td style='padding: 8px; border-bottom: 1px solid #ddd; text-align: right;'>${subTotal}</td>
                    </tr>";
            }

            newOrder.Total = totalAmount;
            _context.Orders.Add(newOrder);
            _context.OrderDetails.AddRange(orderDetails);
            await _context.SaveChangesAsync();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                if (user != null)
                {
                    string asunto = $"Purchase Receipt - Order #{newOrder.Id}";
                    string mensajeHtml = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px; max-width: 600px; margin: 0 auto; border: 1px solid #ddd; border-radius: 8px;'>
                            <h2 style='color: #4CAF50; text-align: center;'>¡Thank you for your purchase., {user.Name}! 🛒</h2>
                            <p>Your order <strong>#{newOrder.Id}</strong> It has been processed successfully. Here is the summary:</p>
                            
                            <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                                <thead>
                                    <tr style='background-color: #f2f2f2;'>
                                        <th style='padding: 10px; text-align: left;'>Product</th>
                                        <th style='padding: 10px; text-align: center;'>Cant.</th>
                                        <th style='padding: 10px; text-align: right;'>Subtotal</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filasFacturaHtml}
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan='2' style='padding: 10px; text-align: right; font-weight: bold; font-size: 18px;'>Total Pagado:</td>
                                        <td style='padding: 10px; text-align: right; font-weight: bold; font-size: 18px; color: #4CAF50;'>${newOrder.Total}</td>
                                    </tr>
                                </tfoot>
                            </table>
                            <p style='text-align: center; margin-top: 30px; color: #777; font-size: 12px;'>This is an automated receipt. Please do not reply to this email.</p>
                        </div>";

                    await _emailService.EnviarCorreoAsync(user.Email, asunto, mensajeHtml);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending receipt email: {ex.Message}");
            }

            return Ok(new
            {
                mensaje = "¡Order successfully created!",
                pedidoId = newOrder.Id,
                totalPagado = newOrder.Total
            });
        }
    }
}
