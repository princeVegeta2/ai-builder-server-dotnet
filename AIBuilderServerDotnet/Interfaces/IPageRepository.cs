using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IPageRepository
    {
        Task AddPage (Page page);

        Task DeletePage(Page page);

        Task<bool> PageExistsForProjectAsync(int projectId, string pageName);

        Task<Page> GetPageByProjectIdAndName(int projectId, string pageName);

        Task UpdatePage(Page page);

    }
}
