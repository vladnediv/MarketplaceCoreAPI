using Domain.Model.Cart;
using Domain.Model.Category;
using Domain.Model.Order;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductMedia> MediaFiles { get; set; }
    public DbSet<DeliveryOption> DeliveryOptions { get; set; }
    public DbSet<ProductCharacteristic> ProductCharacteristics { get; set; }
    public DbSet<ProductQuestion> ProductQuestions { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductQuestionAnswer> ProductQuestionAnswers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Product -> DeliveryOption: One-To-Many relation
        modelBuilder.Entity<Product>()
            .HasMany(product => product.ProductDeliveryOptions)
            .WithOne(deliveryOption => deliveryOption.Product)
            .HasForeignKey(deliveryOption => deliveryOption.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
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
        
        modelBuilder.Entity<ProductMedia>(e =>
        {
            e.HasOne(pm => pm.Product)
                .WithMany(p => p.MediaFiles)
                .HasForeignKey(pm => pm.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pm => pm.ProductReview)
                .WithMany(r => r.MediaFiles)
                .HasForeignKey(pm => pm.ProductReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            e.HasOne(pm => pm.ProductQuestion)
                .WithMany(q => q.MediaFiles)
                .HasForeignKey(pm => pm.ProductQuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        //Category -> Category: Self-referencing relationship
        modelBuilder.Entity<Category>()
            .HasOne(category => category.ParentCategory)
            .WithMany(category => category.Subcategories)
            .HasForeignKey(category => category.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        //Category -> Product: One-To-Many relation
        modelBuilder.Entity<Category>()
            .HasMany(category => category.Products)
            .WithOne(product => product.Category)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        //Order -> OrderItem: One-To-Many relation
        modelBuilder.Entity<Order>()
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //Cart -> CartItem: One-To-Many relation
        modelBuilder.Entity<Cart>()
            .HasMany(cart => cart.CartItems)
            .WithOne(cartItem => cartItem.Cart)
            .HasForeignKey(cartItem => cartItem.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}