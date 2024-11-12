using Microsoft.EntityFrameworkCore;
using Paymant_Module_NEOXONLINE.Contract.Exeptions;

namespace Paymant_Module_NEOXONLINE
{
    internal static class DbInitializer
    {
        public static void InitializeDb(IServiceProvider serviceProvider)
        {
            if (!ApplyMigrations(serviceProvider))
            {
                throw new DbInitializationException("Could not initialize DB! See errors above.");
            }
        }


        private static bool ApplyMigrations(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<Serilog.ILogger>();
            logger.Information("Applying migrations...");

            var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DbContext>();

            try
            {
                context.Database.Migrate();
                logger.Information("Migration(s) applied!");
            }
            catch (Exception ex)
            {
                logger.Error("Database.Migrate failed!");
                logger.Error(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
