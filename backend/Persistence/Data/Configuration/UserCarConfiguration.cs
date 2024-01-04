using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class UserCarConfiguration : IEntityTypeConfiguration<UserCar>
    {
        public void Configure(EntityTypeBuilder<UserCar> builder)
        {
            builder.ToTable("usercar");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);

            builder.Property(x => x.QuantityInCar).HasColumnType("int");

            builder.HasOne(x => x.Users).WithMany(x => x.UserCars).HasForeignKey(x => x.IdUserFk);
            
            builder.HasOne(x => x.Products).WithMany(x => x.UserCars).HasForeignKey(x => x.IdProductFk);
        }
    }
}