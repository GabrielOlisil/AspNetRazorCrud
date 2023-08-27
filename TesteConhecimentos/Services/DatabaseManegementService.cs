using Microsoft.EntityFrameworkCore;
using TesteConhecimentos.Data;

namespace TesteConhecimentos.Services;

public static class DatabaseManegementService
{
    public static void MigrationRun(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var serviceDb = serviceScope.ServiceProvider.GetService<AppDbContext>();
            
            serviceDb.Database.Migrate();
        }
    }
}