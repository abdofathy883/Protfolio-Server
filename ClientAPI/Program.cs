using Core.Interfaces;
using Core.Models;
using Core.Settings;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace ClientAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddDbContext<PortfolioDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnectionString")));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<PortfolioDbContext>();

            builder.Services.Configure<RecaptchaSeetings>(builder.Configuration.GetSection("Recaptcha"));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddHttpClient<IContactFormService, ContactFormService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendOnly", policy =>
                {
                    policy.WithOrigins("https://abdofathy.cloud", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("contact-limit", config =>
                {
                    config.PermitLimit = 5;
                    config.Window = TimeSpan.FromMinutes(1);
                    config.QueueLimit = 0;
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAll");
            app.MapControllers();

            app.Run();
        }
    }
}
