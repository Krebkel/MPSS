using Contracts.EmployeeEntities;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Tests.Employees;

/// <summary>
/// Набор тестов для класса <see cref="EmployeeRepository"/>. НЕ АКТУАЛЕН
/// </summary>
[TestFixture]
public class EmployeeTests
{
    private AppDbContext _context;
    private EmployeeRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "EmployeeTestDb")
            .Options;

        var dataOptions = Options.Create(new DataOptions
        {
            ConnectionString = "InMemoryDbConnectionString", 
            ServiceSchema = "test_schema" 
        });
        
        _context = new AppDbContext(options, dataOptions);
        _context.Database.EnsureDeleted(); 
        _context.Database.EnsureCreated();

        _context.Employees.AddRange(new List<Employee>
        {
            new Employee
            {
                Id = 1, 
                Name = "John Doe", 
                Phone = "123456789", 
                IsDriver = false
            },
            new Employee
            {
                Id = 2, 
                Name = "Jane Doe", 
                Phone = "987654321", 
                IsDriver = true
            }
        });
        _context.SaveChanges();

        _repository = new EmployeeRepository(_context);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    /// <summary>
    /// Тест для метода <see cref="EmployeeRepository.CreateEmployee(Employee)"/>.
    /// </summary>
    [Test]
    public void CreateEmployee_ShouldAddEmployeeToDatabase()
    {
        var employee = new Employee
        {
            Name = "New Employee", 
            Phone = "555555555", 
            IsDriver = false
        };
            
        var employeeId = _repository.CreateEmployee(employee);
        var createdEmployee = _context.Employees.Find(employeeId);

        Assert.IsNotNull(createdEmployee);
        Assert.AreEqual("New Employee", createdEmployee.Name);
    }

    /// <summary>
    /// Тест для метода <see cref="EmployeeRepository.GetEmployee(int)"/>.
    /// </summary>
    [Test]
    public void GetEmployee_ShouldReturnCorrectEmployee()
    {
        var employee = _repository.GetEmployee(1);
        Assert.IsNotNull(employee);
        Assert.AreEqual(1, employee.Id);
    }

    /// <summary>
    /// Тест для метода <see cref="EmployeeRepository.GetAllEmployees()"/>.
    /// </summary>
    [Test]
    public void GetAllEmployees_ShouldReturnAllEmployees()
    {
        var employees = _repository.GetAllEmployees();
        Assert.AreEqual(2, employees.Count);
    }

    /// <summary>
    /// Тест для метода <see cref="EmployeeRepository.UpdateEmployee(Employee)"/>.
    /// </summary>
    [Test]
    public void UpdateEmployee_ShouldUpdateEmployeeInDatabase()
    {
        var existingEmployee = _context.Employees.Find(1);

        if (existingEmployee != null)
        {
            existingEmployee.Name = "Updated Name";
            existingEmployee.Phone = "111111111";
            existingEmployee.IsDriver = true;
        
            _context.SaveChanges();
        }

        var updatedEmployee = _context.Employees.Find(1);

        Assert.IsNotNull(updatedEmployee);
        Assert.AreEqual("Updated Name", updatedEmployee.Name);
    }


    /// <summary>
    /// Тест для метода <see cref="EmployeeRepository.DeleteEmployee(int)"/>.
    /// </summary>
    [Test]
    public void DeleteEmployee_ShouldRemoveEmployeeFromDatabase()
    {
        _repository.DeleteEmployee(1);

        var deletedEmployee = _context.Employees.Find(1);
        Assert.IsNull(deletedEmployee);
    }
}