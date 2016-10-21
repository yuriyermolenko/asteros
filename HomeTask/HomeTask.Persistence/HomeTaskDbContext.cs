using System.Data.Entity;
using HomeTask.Persistence.Mappings;

namespace HomeTask.Persistence
{
    public class HomeTaskDbContext : DbContext
    {
        public HomeTaskDbContext()
            //: base("HomeTaskConnection")
            : base(@"Data Source = (local)\SQLEXPRESS;Initial Catalog = HomeTask;Integrated Security=True")
        {
        }

        public HomeTaskDbContext(IDatabaseInitializer<HomeTaskDbContext> initializer)
            : base("HomeTaskConnection")
        {
            Database.SetInitializer<HomeTaskDbContext>(initializer);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}