
using BLL.OmanDigitalShop.Context;
using BLL.OmanDigitalShop.Repositories;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SLL.OmanDigitalShop.Interfaces;
using SLL.OmanDigitalShop.Services;

namespace Pll.Api.OmanDigitalShop;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // ============================================
        // ????? ?????????
        // ============================================

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

        // ============================================
        // ????? Identity ????????
        // ============================================

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            // ??????? ???? ??????
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;

            // ??????? ????????
            options.User.RequireUniqueEmail = true;

        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        //builder.Services.ConfigureApplicationCookie(options =>
        //{
        //    options.LoginPath = "/Account/Login";
        //    options.LogoutPath = "/Account/Logout";
        //    options.AccessDeniedPath = "/Account/AccessDenied";
        //    options.ExpireTimeSpan = TimeSpan.FromDays(7);
        //});
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddScoped<IProductService, ProductService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
