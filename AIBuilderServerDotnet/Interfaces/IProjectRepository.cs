using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IProjectRepository
    {
        // Creating a project
        Task AddProject (Project project);

        // Checking if a project with the same name exists for that user
        Task<bool> ProjectExistsForUserAsync(int userId, string projectName);

        // Retrieving a project name by user id
        Task<Project> GetProjectByUserIdAndProjectName(int userId, string projectName);
    }
}
