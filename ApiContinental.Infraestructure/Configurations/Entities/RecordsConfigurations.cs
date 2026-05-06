using ApiContinental.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ApiContinental.Infraestructure.Configurations.Entities
{
    public class RecordsConfigurations : IEntityTypeConfiguration<ImcRecord>
    {
        public void Configure(EntityTypeBuilder<ImcRecord> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
