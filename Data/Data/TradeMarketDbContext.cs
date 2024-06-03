using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentException("Invalid ModelBuilder");
            }

            modelBuilder.Entity<Customer>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Customer>()
                .HasOne(x => x.Person)
                .WithMany()
                .HasForeignKey(x => x.PersonId);

            modelBuilder.Entity<Person>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Product>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Product>()
                  .HasOne(d => d.Category)
                  .WithMany(p => p.Products)
                  .HasForeignKey(d => d.ProductCategoryId);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Receipt>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Receipt>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Receipts)
                .HasForeignKey(x =>x.CustomerId);

            modelBuilder.Entity<ReceiptDetail>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(x => x.Product)
                .WithMany(x => x.ReceiptDetails)
                .HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(x => x.Receipt)
                .WithMany(x => x.ReceiptDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.ReceiptId);
        }
    }
}
