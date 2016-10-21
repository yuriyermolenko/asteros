using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using HomeTask.Domain.Aggregates.ClientAgg;

namespace HomeTask.Persistence.Mappings
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            ToTable(MappingConstants.Tables.Clients);

            HasKey(e => e.Id);
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Address).HasMaxLength(100).IsRequired();
        }
    }
}