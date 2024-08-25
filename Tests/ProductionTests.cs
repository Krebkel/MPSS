using Contracts;
using Contracts.EmployeeEntities;
using Contracts.ProductEntities;
using Contracts.ProjectEntities;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Tests;

[TestFixture]
public class ProductionTests
{
    private AppDbContext _context;
    private EmployeeRepository _employeeRepository;
    private EmployeeShiftRepository _employeeShiftRepository;
    private CounteragentRepository _counteragentRepository;
    private ProjectRepository _projectRepository;
    private ProductRepository _productRepository;
    private ProductComponentRepository _productComponentRepository;
    private ProjectProductRepository _projectProductRepository;
    private ExpenseRepository _expenseRepository;
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductTestDb")
            .Options;

        var dataOptions = Options.Create(new DataOptions 
        { 
            ConnectionString = "InMemoryDbConnectionString", 
            ServiceSchema = "test_schema" 
        });
        
        _context = new AppDbContext(options, dataOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _employeeShiftRepository = new EmployeeShiftRepository(_context);
        _projectRepository = new ProjectRepository(_context, _employeeShiftRepository);

        SeedTestData();
    }

    private void SeedTestData()
    {
        var counteragentA = new Counteragent
        {
            Name = "Counteragent A",
            Contact = "Contact A",
            Phone = "Phone A",
            INN = 12345,
            OGRN = 12345,
            AccountNumber = 12345,
            BIK = 12345
        };

        var products = new[]
        {
            new Product { Name = "Product A", Cost = 10000 },
            new Product { Name = "Product B", Cost = 5000 }
        };

        var projects = new[]
        {
            new Project 
            { 
                Name = "Project A", 
                CounteragentId = 1, 
                DeadlineDate = DateTimeOffset.Now.AddDays(10), 
                Address = "Korolyov", 
                ProjectStatus = ProjectStatus.Done,
                ManagerShare = 5 // 5% руководителю
            },
            new Project 
            { 
                Name = "Project B", 
                CounteragentId = 1, 
                DeadlineDate = DateTimeOffset.Now.AddDays(5), 
                Address = "Chekhov", 
                ProjectStatus = ProjectStatus.Done,
                ManagerShare = 10 // 10% руководителю
            },
            new Project 
            { 
                Name = "Project C", 
                CounteragentId = 1, 
                DeadlineDate = DateTimeOffset.Now.AddDays(1), 
                Address = "Vykhino", 
                ProjectStatus = ProjectStatus.Done,
                ManagerShare = 7 // 7% руководителю
            }
        };

        var projectProducts = new[]
        {
            new ProjectProduct 
            { 
                ProjectId = 1, 
                ProductId = 1, 
                Quantity = 50, 
                Markup = 300 
            },
            new ProjectProduct 
            { 
                ProjectId = 1, 
                ProductId = 2, 
                Quantity = 10, 
                Markup = 400 
            },
            new ProjectProduct 
            { 
                ProjectId = 2, 
                ProductId = 1, 
                Quantity = 100, 
                Markup = 200 
            },
            new ProjectProduct 
            { 
                ProjectId = 2, 
                ProductId = 2, 
                Quantity = 20, 
                Markup = 500 
            },
            new ProjectProduct 
            { 
                ProjectId = 3, 
                ProductId = 1, 
                Quantity = 150, 
                Markup = 300 
            }
        };

        var employees = new[]
        {
            new Employee 
            { 
                Name = "Employee A", 
                Phone = "+2(456)890-23-56", 
                IsDriver = true, 
                Passport = 4620999999, 
                DateOfBirth = DateTimeOffset.Now.AddYears(-21), 
                INN = 123456789012, 
                AccountNumber = 12345678901234567890, 
                BIK = 123456789 
            },
            new Employee 
            { 
                Name = "Employee B", 
                Phone = "+5(321)987-54-21", 
                IsDriver = false, 
                DateOfBirth = DateTimeOffset.Now.AddYears(-26) 
            }
        };

        var employeeShifts = new[]
        {
            new EmployeeShift 
            { 
                ProjectId = 1, 
                EmployeeId = 1, 
                Date = DateTimeOffset.Now, 
                Arrival = DateTimeOffset.Now.AddHours(-10), 
                Departure = DateTimeOffset.Now, 
                TravelTime = 2.5f, 
                ConsiderTravel = false, 
                ISN = 5 
            },
            new EmployeeShift 
            { 
                ProjectId = 1, 
                EmployeeId = 2, 
                Date = DateTimeOffset.Now, 
                Arrival = DateTimeOffset.Now.AddHours(-10), 
                Departure = DateTimeOffset.Now, 
                ConsiderTravel = false, 
                ISN = 3
            },
            new EmployeeShift 
            { 
                ProjectId = 2, 
                EmployeeId = 1, 
                Date = DateTimeOffset.Now.AddHours(-24), 
                Arrival = DateTimeOffset.Now.AddHours(-34), 
                Departure = DateTimeOffset.Now.AddHours(-24), 
                TravelTime = 2.5f, 
                ConsiderTravel = false, 
                ISN = 1 
            },
            new EmployeeShift 
            { 
                ProjectId = 3, 
                EmployeeId = 1, 
                Date = DateTimeOffset.Now, 
                Arrival = DateTimeOffset.Now.AddHours(-10), 
                Departure = DateTimeOffset.Now, 
                TravelTime = 2.5f, 
                ConsiderTravel = true, 
                ISN = 5 
            },
            new EmployeeShift 
            { 
                ProjectId = 3, 
                EmployeeId = 2, 
                Date = DateTimeOffset.Now, 
                Arrival = DateTimeOffset.Now.AddHours(-10), 
                Departure = DateTimeOffset.Now, 
                ConsiderTravel = false, 
                ISN = 4 
            }
        };

        var expenses = new[]
        {
            new Expense
            {
                ProjectId = 1,
                Name = "Tickets",
                Amount = 10000,
                Type = ExpenseType.Travel,
                IsPaidByCompany = true
            },
            new Expense
            {
                ProjectId = 1,
                Name = "Food",
                Amount = 5000,
                Type = ExpenseType.Other,
                IsPaidByCompany = false
            }
        };

        _context.Counteragents.Add(counteragentA);
        _context.Products.AddRange(products);
        _context.Projects.AddRange(projects);
        _context.ProjectProducts.AddRange(projectProducts);
        _context.Employees.AddRange(employees);
        _context.EmployeeShifts.AddRange(employeeShifts);
        _context.Expenses.AddRange(expenses);
        
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }
    
    [Test]
    public void CalculateTotalWageForDoneProjects_ShouldReturnCorrectWage()
    {
        int employeeId = 1;
        
        // Проект 1
        double project1TotalMarkup = 50 * 300 + 10 * 400; // 15000 + 4000 = 19000
        double project1ManagerShare = project1TotalMarkup * 0.05; // 5% от общей наценки = 950
        double project1ExpensesNotPaidByCompany = 5000;
        double project1DistributableAmount = project1TotalMarkup 
                                            - project1ManagerShare 
                                            - project1ExpensesNotPaidByCompany; // 19000 - 950 - 5000 = 13050

        // Расчет для сотрудника 1 по проекту 1
        double employee1ShiftHours1 = 10; // Рабочие часы
        double employee1ISN1 = 5;
        double employee2ISN1 = 3;
        double totalISN1 = employee1ISN1 + employee2ISN1; // 5 + 3 = 8
        double employee1BaseWage1 = 300 * employee1ShiftHours1; // 300 * 10 = 3000
        double employee1Bonus1 = (employee1ISN1 / totalISN1) * project1DistributableAmount; // (5 / 8) * 13050 = 8156.25
        double expectedEmployee1Wage1 = employee1BaseWage1 + employee1Bonus1; // 3000 + 8156.25 = 11156.25

        // Проект 2
        double project2TotalMarkup = 100 * 200 + 20 * 500; // 20000 + 10000 = 30000
        double project2ManagerShare = project2TotalMarkup * 0.10; // 10% от общей наценки = 3000
        double project2DistributableAmount = project2TotalMarkup - project2ManagerShare; // 30000 - 3000 = 27000

        // Расчет для сотрудника 1 по проекту 2
        double employee1ShiftHours2 = 10; // Рабочие часы
        double employee1ISN2 = 1; // Единственный сотрудник на проекте
        double totalISN2 = employee1ISN2; // 1
        double employee1BaseWage2 = 300 * employee1ShiftHours2; // 300 * 10 = 3000
        double employee1Bonus2 = (employee1ISN2 / totalISN2) * project2DistributableAmount; // (1 / 1) * 27000 = 27000
        double expectedEmployee1Wage2 = employee1BaseWage2 + employee1Bonus2; // 3000 + 27000 = 30000

        // Проект 3
        double project3TotalMarkup = 150 * 300; // 45000
        double project3ManagerShare = project3TotalMarkup * 0.07; // 7% от общей наценки = 3150
        double project3DistributableAmount = project3TotalMarkup - project3ManagerShare; // 45000 - 3150 = 41850

        // Расчет для сотрудника 1 по проекту 3
        double employee1ShiftHours3 = 10; // Рабочие часы
        double employee1ISN3 = 5;
        double employee2ISN3 = 4;
        double totalISN3 = employee1ISN3 + employee2ISN3; // 5 + 4 = 9
        double employee1BaseWage3 = 300 * employee1ShiftHours3; // 300 * 10 = 3000
        double employee1Bonus3 = (employee1ISN3 / totalISN3) * project3DistributableAmount; // (5 / 9) * 41850 = 23250
        double expectedEmployee1Wage3 = employee1BaseWage3 + employee1Bonus3; // 3000 + 23250 = 26250

        // Общая ожидаемая зарплата для сотрудника 1 по всем завершенным проектам
        double expectedTotalWage = expectedEmployee1Wage1 + expectedEmployee1Wage2 + expectedEmployee1Wage3;
        // 11156.25 + 30000 + 26250 = 67406.25

        var actualTotalWage = _projectRepository.CalculateTotalWageForDoneProjects(employeeId);

        Assert.AreEqual(expectedTotalWage, actualTotalWage, 0.01);
        Console.WriteLine($"Expected: {expectedTotalWage}, Actual: {actualTotalWage}");
    }
}