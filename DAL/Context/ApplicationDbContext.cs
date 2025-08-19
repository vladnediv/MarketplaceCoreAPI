using Domain.Model.Cart;
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
    public DbSet<Cart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Product -> ProductDeliveryOption <- DeliveryOption : Many-To-Many relation
        modelBuilder.Entity<ProductDeliveryOption>()
            .HasKey(pdo => new { pdo.ProductId, pdo.DeliveryOptionId });

        modelBuilder.Entity<ProductDeliveryOption>()
            .HasOne(pdo => pdo.Product)
            .WithMany(p => p.ProductDeliveryOptions)
            .HasForeignKey(pdo => pdo.ProductId);

        modelBuilder.Entity<ProductDeliveryOption>()
            .HasOne(pdo => pdo.DeliveryOption)
            .WithMany(d => d.ProductDeliveryOptions)
            .HasForeignKey(pdo => pdo.DeliveryOptionId);
        
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
        
        //Order -> OrderItem: One-To-Many relation
        modelBuilder.Entity<Order>()
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}