using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Data;

public static class DbInitializer
{
    // Force-drops the database even if connections are open, then lets EF recreate it.
    public static void ForceRecreate(AppDbContext db, IConfiguration config)
    {
        var conn = config.GetConnectionString("DefaultConnection")!;

        // Build a connection to the 'master' db to drop CentalDb safely.
        var builder = new SqlConnectionStringBuilder(conn);
        var dbName = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        try
        {
            using var master = new SqlConnection(builder.ConnectionString);
            master.Open();
            var sql = $@"
                IF DB_ID('{dbName}') IS NOT NULL
                BEGIN
                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{dbName}];
                END";
            using var cmd = new SqlCommand(sql, master);
            cmd.ExecuteNonQuery();
        }
        catch
        {
            // fall back to EF's own delete if the raw drop fails
            db.Database.EnsureDeleted();
        }

        db.Database.EnsureCreated();
    }
}
