using Contracts.Employee;
using Contracts.Product;
using Contracts.Project;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectProduct = Contracts.Product.ProjectProduct;

namespace Data;

public class AppDbContext : DbContext
{
    private readonly IOptions<DataOptions> _dataOptions;

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<EmployeeShift> EmployeeShifts { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductComponent> ProductComponents { get; set; } = null!;
    public DbSet<Counteragent> Counteragents { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectProduct> ProjectProducts { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<DataOptions> dataOptions) : base(options)
    {
        _dataOptions = dataOptions;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(_dataOptions.Value.ServiceSchema);
    }
}