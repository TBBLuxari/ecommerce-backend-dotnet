using Api.Data; 
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api 
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {  
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            bool adminExists = await context.Users.AnyAsync(u => u.Rol == "Admin");

            if (!adminExists)
            {
                var adminUser = new User
                {
                    Name = "Administrador Principal",
                    Email = "admin@mitienda.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Rol = "Admin",
                    IsVerified = true 
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();

                Console.WriteLine("🌱 Data Seeding: Administrator user successfully created.");
            }
        }
    }
}