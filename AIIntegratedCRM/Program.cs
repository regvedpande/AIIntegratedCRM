using AIIntegratedCRM.Data;
using AIIntegratedCRM.Repositories.Implementations;
using AIIntegratedCRM.Repositories.Interfaces;
using AIIntegratedCRM.Services.Implementations;
using AIIntegratedCRM.Services.Interfaces;
using Microsoft.EntityFrameworkCore;  // Required for UseSqlServer

var builder = WebApplication.CreateBuilder(args);

// 1. Add controllers with views
builder.Services.AddControllersWithViews();

// 2. Configure EF Core to use SQL Server (AIIntegratedCRMDB)
//    Note: "AIIntegratedCRMDB" must be defined under ConnectionStrings in appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AIIntegratedCRMDB")));

// 3. Register Repositories and Services for dependency injection
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAIService, AIService>();

var app = builder.Build();

// 4. Ensure that the database exists and that EF has created any necessary tables
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //db.Database.EnsureCreated();
}

// 5. Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 6. Default route: land on Customer/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

app.Run();
