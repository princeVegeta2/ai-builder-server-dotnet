using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IProjectRepository
    {
        // Creating a project
        Task AddProject (Project project);
    }
}
