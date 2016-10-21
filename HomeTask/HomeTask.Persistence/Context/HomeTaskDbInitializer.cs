using System.Data.Entity;
using HomeTask.Persistence.Migrations;

namespace HomeTask.Persistence.Context
{
    public class HomeTaskDbInitializer : IDatabaseInitializer<HomeTaskDbContext>
    {
        public void InitializeDatabase(HomeTaskDbContext context)
        {
            var createDb = new MigrateDatabaseToLatestVersion<HomeTaskDbContext, Configuration>();
            createDb.InitializeDatabase(context);
        }
    }
}
