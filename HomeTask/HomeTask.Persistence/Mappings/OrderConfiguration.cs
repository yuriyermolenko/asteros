using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using HomeTask.Domain.Aggregates.OrderAgg;

namespace HomeTask.Persistence.Mappings
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable(MappingConstants.Tables.Orders);

            HasKey(e => e.Id);
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Description).HasMaxLength(200).IsRequired();

            HasRequired(c => c.Client)
                .WithMany(o => o.Orders)
                .HasForeignKey(c => c.ClientId).WillCascadeOnDelete(false);
        }
    }
}