using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Domain
{
    public class RolesSeed : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "Viewer"
                },
                new Role
                {
                    Id = 2,
                    Name = "Subscriber"
                },
                new Role
                {
                    Id = 3,
                    Name = "Admin"
                }
            );
        }
    }
}
