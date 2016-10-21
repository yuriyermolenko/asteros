using System.Data.Entity;
using HomeTask.Persistence.Mappings;

namespace HomeTask.Persistence
{
    public class HomeTaskDbContext : DbContext
    {
        public HomeTaskDbContext()
            : base("HomeTaskConnection")
        {
        }

        public HomeTaskDbContext(IDatabaseInitializer<HomeTaskDbContext> initializer)
            : base("HomeTaskConnection")
        {
            Database.SetInitializer<HomeTaskDbContext>(initializer);
        }

        #region Protected methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}