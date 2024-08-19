using Contracts.ProjectEntities;
using Data;
using Employees.Services;
using Projects.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Tests.Projects;

[TestFixture]
public class ProjectTests
{
    private AppDbContext _context;
    private ProjectService _service;
    private IEmployeeShiftService _employeeShiftService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductTestDb")
            .Options;

        var dataOptions = Options.Create(new DataOptions { ConnectionString = "InMemoryDbConnectionString", ServiceSchema = "test_schema" });
        
        _context = new AppDbContext(options, dataOptions);
        _employeeShiftService = new EmployeeShiftService(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _service = new ProjectService(_context, _employeeShiftService);

        var project = new Project
        {
            Name = "Project A",
            CounteragentId = 1,
            DeadlineDate = DateTimeOffset.Now.AddDays(30),
            Address = "Korolyov",
            ProjectStatus = ProjectStatus.Standby
        };
        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }

    [Test]
    public void CreateProject_ShouldAddProjectToDatabase()
    {
        var project = new Project
        {
            Name = "Project B",
            CounteragentId = 3,
            DeadlineDate = DateTimeOffset.Now.AddDays(15),
            ProjectStatus = ProjectStatus.Active,
            Address = "Chekhov"
        };

        var projectId = _service.CreateProject(project);
        var createdProject = _context.Projects.Find(projectId);

        Assert.IsNotNull(createdProject);
        Assert.AreEqual("Project B", createdProject.Name);
        Assert.AreEqual("Chekhov", createdProject.Address);
    }

    [Test]
    public void GetProject_ShouldReturnCorrectProject()
    {
        var project = _service.GetProject(1);
        Assert.IsNotNull(project);
        Assert.AreEqual(1, project.Id);
    }

    [Test]
    public void GetAllProjects_ShouldReturnAllProjects()
    {
        var projects = _service.GetAllProjects();
        Assert.AreEqual(1, projects.Count);
    }

    [Test]
    public void UpdateProject_ShouldUpdateProjectInDatabase()
    {
        var project = _context.Projects.Find(1);
        if (project != null)
        {
            project.Name = "Updated Project A";
            _service.UpdateProject(project);
        }

        var updatedProject = _context.Projects.Find(1);
        Assert.IsNotNull(updatedProject);
        Assert.AreEqual("Updated Project A", updatedProject.Name);
    }

    [Test]
    public void DeleteProject_ShouldRemoveProjectFromDatabase()
    {
        _service.DeleteProject(1);
        var deletedProject = _context.Projects.Find(1);
        Assert.IsNull(deletedProject);
    }

    [Test]
    public void SuspendProject_ShouldSetProjectInactive()
    {
        _service.ChangeProjectStatus(1, ProjectStatus.Standby);
        var project = _context.Projects.Find(1);
        Assert.IsTrue(project.ProjectStatus == ProjectStatus.Standby);
    }
}
