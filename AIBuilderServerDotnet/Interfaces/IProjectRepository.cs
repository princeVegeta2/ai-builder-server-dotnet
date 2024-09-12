using AIBuilderServerDotnet.DTOs;
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
        // Retrieving all projects for the user
        Task<IEnumerable<Project>> GetProjectsByUserId(int userId);
        // Retrieving all project data for a single project
        Task<ProjectDto> GetProjectDetails(int userId, string projectName);
        // Deleting a project
        Task DeleteProject(Project project);

    }
}
