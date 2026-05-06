using ApiContinental.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ApiContinental.Infraestructure.Configurations.Entities
{
    public class CategoriesConfigurations : IEntityTypeConfiguration<ImcCategory>
    {
        public void Configure(EntityTypeBuilder<ImcCategory> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
