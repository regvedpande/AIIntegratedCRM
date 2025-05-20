namespace AIIntegratedCRM.Data
{
    using AIIntegratedCRM.Models.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // EF will create a "Customers" table in AIIntegratedCRMDB
        public DbSet<Customer> Customers { get; set; }
    }
}
