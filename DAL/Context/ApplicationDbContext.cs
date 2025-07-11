using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<DeliveryOption> DeliveryOptions { get; set; }
    public DbSet<ProductDescription> ProductDescriptions { get; set; }
    public DbSet<ProductDeliveryOption> ProductDeliveryOptions { get; set; }
    public DbSet<ProductQuestion> ProductQuestions { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductVariation> ProductVariations { get; set; }
    public DbSet<CurrentVariation> CurrentVariations { get; set; }

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
    }
}