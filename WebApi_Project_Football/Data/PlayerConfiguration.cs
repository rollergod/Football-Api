using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Data
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder
               .Property(p => p.FirstName).IsRequired();
            builder
               .Property(p => p.LastName).IsRequired();
            builder
              .Property(p => p.Position).IsRequired();
        }
    }
}
