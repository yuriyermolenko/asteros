using System.Data.Entity;
using HomeTask.Persistence.Mappings;

namespace HomeTask.Persistence.Context
{
    public class HomeTaskDbContext : DbContext, IHomeTaskDbContext
    {
        public HomeTaskDbContext()
            : base("HomeTaskConnection")
        {
            Database.SetInitializer(new HomeTaskDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}