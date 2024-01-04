using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);

            builder.Property(x => x.Username).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Password).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(80);

            builder.Property(x => x.RefreshToken);

            builder.Property(x => x.TokenCreated).HasColumnType("date");

            builder.Property(x => x.TokenExpires).HasColumnType("date");

            builder.HasMany(u => u.Rols).WithMany(r => r.Users).UsingEntity<UserRol>(
                x => x.HasOne(ur => ur.Rols).WithMany(r => r.UserRols).HasForeignKey(ur => ur.IdRolFk),
                x => x.HasOne(ur => ur.Users).WithMany(u => u.UserRols).HasForeignKey(ur => ur.IdUserFk),
                x =>
                {
                    x.ToTable("userrol");
                    x.HasKey(y => new { y.IdUserFk, y.IdRolFk });
                }
            );
        }
    }
}