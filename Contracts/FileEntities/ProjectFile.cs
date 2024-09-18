using Contracts.ProjectEntities;
using DataContracts;

namespace Contracts.FileEntities;

public class ProjectFile : DatabaseEntity
{
    public string Name { get; set; }
    public string FilePath { get; set; }
    public Project? Project { get; set; }
    public DateTimeOffset UploadDate { get; set; }
}