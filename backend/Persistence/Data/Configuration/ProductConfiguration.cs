using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("product");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Image).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Size).IsRequired().HasMaxLength(5);

            builder.Property(x => x.Price).HasColumnType("double");

            builder.Property(x => x.Stock).HasColumnType("int");

            builder.Property(x => x.StockMin).HasColumnType("int");

            builder.Property(x => x.StockMax).HasColumnType("int");

            builder.HasOne(x => x.ProductTypes).WithMany(x => x.Products).HasForeignKey(x => x.IdProductTypeFk);

            builder.HasOne(x => x.States).WithMany(x => x.Products).HasForeignKey(x => x.IdStateFk);
        }
    }
}