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

            HasKey(c => c.Id);
            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Address).HasMaxLength(100).IsRequired();

            HasMany(c => c.Orders).WithRequired(o => o.Client).HasForeignKey(o => o.ClientId);
        }
    }
}