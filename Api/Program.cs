using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using System.Text;
using Api.Services;
using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors( options => 
{
    options.AddPolicy("PermitirFrontend", policy => { policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod(); });
});
builder.Services.AddScoped<IEmailService, EmailService>();


var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(key), 
            ValidateIssuer = false, 
            ValidateAudience = false 
        };
    });



var app = builder.Build();
await DbSeeder.SeedAdminAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("PermitirFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();