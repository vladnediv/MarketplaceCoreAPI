using System.Xml.Schema;
using BLL.Service;
using BLL.Service.Interface;
using BLL.Service.Mappings;
using DAL.Context;
using DAL.Repository;
using DAL.Repository.Interface;
using Domain.Model.Order;
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
        

        app.UseCors("AllowAll");

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        //Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("MarketplaceCoreDb"));
        });
        
        //Add AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        
        //configure cors for front-end
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        //Dependency Injection registration
        //repository
        builder.Services.AddScoped<IAdvancedRepository<DeliveryOption>, DeliveryOptionRepository>();
        builder.Services.AddScoped<IAdvancedRepository<Order>, OrderRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductCharacteristic>, ProductCharacteristicRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductQuestion>, ProductQuestionRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductQuestionAnswer>, ProductQuestionAnswerRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductReview>, ProductReviewRepository>();
        
        //services
        builder.Services.AddScoped<IAdvancedService<DeliveryOption>, DeliveryOptionService>();
        builder.Services.AddScoped<IAdvancedService<ProductCharacteristic>, ProductCharacteristicService>();
        builder.Services.AddScoped<IGenericService<ProductQuestionAnswer>, ProductQuestionAnswerService>();
        
        builder.Services.AddScoped<IAdvancedService<ProductQuestion>, ProductQuestionService>();
        builder.Services.AddScoped<IGenericService<ProductQuestion>, ProductQuestionService>();
        
        builder.Services.AddScoped<IAdvancedService<ProductReview>, ProductReviewService>();
        builder.Services.AddScoped<IGenericService<ProductReview>, ProductReviewService>();
        
        builder.Services.AddScoped<IProductService, ProductService>();
        
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
        builder.Services.AddScoped<IShopService, ShopService>();
    }
}