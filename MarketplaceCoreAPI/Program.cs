using System;
using System.Text;
using System.Xml.Schema;
using BLL.Service;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Mappings;
using BLL.Service.ServiceHelpers;
using DAL.Context;
using DAL.Repository;
using DAL.Repository.CartRepositories;
using DAL.Repository.CategoryRepositories;
using DAL.Repository.Interface;
using DAL.Repository.OrderRepositories;
using DAL.Repository.ProductRepositories;
using Domain.Model.Cart;
using Domain.Model.Category;
using Domain.Model.Order;
using Domain.Model.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using FileService = BLL.Service.ServiceHelpers.FileService;
using ProductService = BLL.Service.ServiceHelpers.ProductService;

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
        builder.Services.AddMemoryCache();
        
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

        app.UseAuthentication();
        app.UseAuthorization();

        //configure wwwroot
        app.UseStaticFiles();

        app.MapControllers();

        app.Run();
        
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        StripeConfiguration.ApiKey = builder.Configuration["StripePay:SecretKey"];
        
        builder.Services.AddAuthorization();
        
        //add swagger configuration for JWT
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketplaceCoreAPI", Version = "V1" });
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "ENTER 'Bearer':"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id ="Bearer"
                        }
                    },
                    new string[]{ }
                }
            });

        });
        
        //JWT and Identity Configuration
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(options =>
        {
            //Only for development
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,

                ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                ValidAudience = builder.Configuration["JwtConfig:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]))
            };
        });
        
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
        builder.Services.AddScoped<IAdvancedRepository<OrderItem>, OrderItemRepository>();
        
        builder.Services.AddScoped<IAdvancedRepository<Cart>, CartRepository>();
        builder.Services.AddScoped<IGenericRepository<CartItem>, CartItemRepository>();
        
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        builder.Services.AddScoped<IAdvancedRepository<ProductCharacteristic>, ProductCharacteristicRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductQuestion>, ProductQuestionRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductQuestionAnswer>, ProductQuestionAnswerRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IAdvancedRepository<ProductReview>, ProductReviewRepository>();
        
        //services
        builder.Services.AddScoped<IAdvancedService<DeliveryOption>, DeliveryOptionService>();
        builder.Services.AddScoped<IAdvancedService<ProductCharacteristic>, ProductCharacteristicService>();
        builder.Services.AddScoped<IGenericService<ProductQuestionAnswer>, ProductQuestionAnswerService>();
        
        
        builder.Services.AddScoped<IAdvancedService<Order>, OrderService>();
        builder.Services.AddScoped<IAdvancedService<OrderItem>, OrderItemService>();
        
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        
        builder.Services.AddScoped<IAdvancedService<Cart>, CartService>();
        builder.Services.AddScoped<IGenericService<CartItem>, CartItemService>();
        
        builder.Services.AddScoped<IAdvancedService<ProductQuestion>, ProductQuestionService>();
        builder.Services.AddScoped<IGenericService<ProductQuestion>, ProductQuestionService>();
        
        builder.Services.AddScoped<IAdvancedService<ProductReview>, ProductReviewService>();
        builder.Services.AddScoped<IGenericService<ProductReview>, ProductReviewService>();
        
        builder.Services.AddScoped<IProductService, ProductService>();
        
        builder.Services.AddScoped<IFileService, FileService>();
        
        builder.Services.AddScoped<IStripeService, StripeService>();
        
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
        builder.Services.AddScoped<IShopService, ShopService>();
    }
}