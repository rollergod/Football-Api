using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Data
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
       

        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                 .Property(t => t.TeamName).IsRequired();
        }
    }
}