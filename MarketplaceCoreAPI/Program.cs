using System.Xml.Schema;
using BLL.Service;
using BLL.Service.Interface;
using DAL.Context;
using DAL.Repository;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        ConfigureServices(builder);
        
        var app = builder.Build();



// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }



        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }

    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        //Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("MarketplaceCoreDb"));
        });
        
        //Dependency Injection registration
        //repository
        builder.Services.AddScoped<DeliveryOptionRepository, DeliveryOptionRepository>();
        builder.Services.AddScoped<ProductCharacteristicRepository, ProductCharacteristicRepository>();
        builder.Services.AddScoped<ProductQuestionRepository, ProductQuestionRepository>();
        builder.Services.AddScoped<ProductRepository, ProductRepository>();
        builder.Services.AddScoped<ProductReviewRepository, ProductReviewRepository>();
        
        //services
        builder.Services.AddScoped<DeliveryOptionService, DeliveryOptionService>();
        builder.Services.AddScoped<ProductCharacteristicService, ProductCharacteristicService>();
        builder.Services.AddScoped<ProductQuestionService, ProductQuestionService>();
        builder.Services.AddScoped<ProductService, ProductService>();
        builder.Services.AddScoped<ProductReviewService, ProductReviewService>();
    }
}