using System.Data.Entity;
using HomeTask.Persistence.Mappings;

namespace HomeTask.Persistence.Context
{
    public class HomeTaskDbContext : DbContext, IHomeTaskDbContext
    {
        public HomeTaskDbContext()
            : base("HomeTaskConnection")
        //: base(@"Data Source = (local)\SQLEXPRESS;Initial Catalog = HomeTask;Integrated Security=True")
        {
            Database.SetInitializer<HomeTaskDbContext>(new HomeTaskDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}