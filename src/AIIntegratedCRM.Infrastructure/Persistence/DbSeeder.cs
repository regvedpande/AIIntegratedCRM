using AIIntegratedCRM.Domain.Entities;
using AIIntegratedCRM.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AIIntegratedCRM.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.Tenants.AnyAsync())
            {
                var tenantId = Guid.NewGuid();
                var tenant = new Tenant
                {
                    Id = tenantId,
                    Name = "Demo Company",
                    Subdomain = "demo",
                    AdminEmail = "admin@demo.com",
                    IsActive = true,
                    Plan = "Enterprise"
                };

                var adminUser = new AppUser
                {
                    TenantId = tenantId,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@demo.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsActive = true
                };

                var account = new Account
                {
                    TenantId = tenantId,
                    Name = "Acme Corporation",
                    Industry = "Technology",
                    Website = "https://acme.com",
                    Phone = "+1-555-0100",
                    City = "San Francisco",
                    Country = "US",
                    AnnualRevenue = 5000000,
                    EmployeeCount = 250,
                    CreatedBy = "system"
                };

                var contact = new Contact
                {
                    TenantId = tenantId,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@acme.com",
                    Phone = "+1-555-0101",
                    JobTitle = "VP of Sales",
                    Company = "Acme Corporation",
                    AccountId = account.Id,
                    CreatedBy = "system"
                };

                var lead = Lead.Create("John", "Doe", "john.doe@example.com", tenantId, "system");
                lead.Company = "TechStartup Inc";
                lead.JobTitle = "CTO";
                lead.Source = LeadSource.Website;
                lead.AIScore = 78;
                lead.AIScoreReason = "High value prospect based on company size and role";

                var opportunity = new Opportunity
                {
                    TenantId = tenantId,
                    Name = "Enterprise CRM Deal",
                    AccountId = account.Id,
                    PrimaryContactId = contact.Id,
                    Stage = OpportunityStage.Qualification,
                    Amount = 120000,
                    Currency = "USD",
                    Probability = 40,
                    AIProbability = 45,
                    ExpectedCloseDate = DateTime.UtcNow.AddMonths(3),
                    CreatedBy = "system"
                };

                await context.Tenants.AddAsync(tenant);
                await context.Users.AddAsync(adminUser);
                await context.Accounts.AddAsync(account);
                await context.Contacts.AddAsync(contact);
                await context.Leads.AddAsync(lead);
                await context.Opportunities.AddAsync(opportunity);
                await context.SaveChangesAsync();

                logger.LogInformation("Database seeded successfully");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
}
