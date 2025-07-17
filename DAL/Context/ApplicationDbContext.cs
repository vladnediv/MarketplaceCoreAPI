using Domain.Model.Order;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductMedia> MediaFiles { get; set; }
    public DbSet<DeliveryOption> DeliveryOptions { get; set; }
    public DbSet<ProductCharacteristic> ProductCharacteristics { get; set; }
    public DbSet<ProductDeliveryOption> ProductDeliveryOptions { get; set; }
    public DbSet<ProductQuestion> ProductQuestions { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductQuestionAnswer> ProductQuestionAnswers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        
        //Product <-> DeliveryOption: Many-To-Many relation
        modelBuilder
            .Entity<Product>()
            .HasMany(a => a.DeliveryOptions)
            .WithMany(a => a.Products)
            .UsingEntity<ProductDeliveryOption>(
                r => r
                .HasOne(a => a.DeliveryOption)
                .WithMany()
                .HasForeignKey(a => a.DeliveryOptionId),
                
                l => l
                    .HasOne(a => a.Product)
                    .WithMany()
                    .HasForeignKey(a => a.ProductId)
            );
        
        //ProductCharacteristic -> Product: One-To-Many relation
        modelBuilder.Entity<Product>()
            .HasMany(product => product.Characteristics)
            .WithOne(characteristic => characteristic.Product)
            .HasForeignKey(characteristic => characteristic.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //ProductReview -> Product: One-To-Many relation
        modelBuilder.Entity<Product>()
            .HasMany(product => product.Reviews)
            .WithOne(review => review.Product)
            .HasForeignKey(review => review.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //ProductQuestion -> Product: One-To-Many relation
        modelBuilder.Entity<Product>()
            .HasMany(product => product.Questions)
            .WithOne(question => question.Product)
            .HasForeignKey(question => question.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //ProductQuestion -> ProductQuestionAnswer: One-To-Many relation
        modelBuilder.Entity<ProductQuestion>()
            .HasMany(productQuestion => productQuestion.Answers)
            .WithOne(answer => answer.Question)
            .HasForeignKey(answer => answer.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //KeyValue -> ProductCharacteristic: One-To-Many relation
        modelBuilder.Entity<ProductCharacteristic>()
            .HasMany(characteristic => characteristic.Characteristics)
            .WithOne(characteristic => characteristic.ProductCharacteristic)
            .HasForeignKey(characteristic => characteristic.ProductCharacteristicId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //ProductMedia -> Product: One-To-Many relation
        modelBuilder.Entity<Product>()
            .HasMany(product => product.MediaFiles)
            .WithOne(mediaFile => mediaFile.Product)
            .HasForeignKey(mediaFile => mediaFile.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //Product <-> OrderItem: One-To-Many relation
        modelBuilder
            .Entity<Product>()
            .HasMany(product => product.OrderItems)
            .WithOne(orderItem => orderItem.Product)
            .HasForeignKey(orderItem => orderItem.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //Order -> OrderItem: One-To-Many relation
        modelBuilder.Entity<Order>()
            .HasMany(order => order.OrderItems)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}