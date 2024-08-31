using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IModalRepository
    {
        Task AddColorModal(ColorModal colorModal);
        Task AddLinkModal(LinkModal linksModal);
        Task AddImageLinkModal(ImageLinkModal imageLinkModal);
        Task AddPromptModal(PromptModal promptModal);
        Task DeleteColorModal(ColorModal colorModal);
        Task DeleteLinkModal(LinkModal linksModal);
        Task DeleteImageLinkModal(ImageLinkModal imageLinkModal);
        Task DeletePromptModal (PromptModal promptModal);
        Task UpdateModalPositionsGlobal(int widgetId, int position);
        Task<ColorModal> GetColorModalByWidgetIdAndPosition(int widgetId, int position);
        Task<LinkModal> GetLinkModalByWidgetIdAndPosition(int widgetId, int position);
        Task<ImageLinkModal> GetImageLinkModalByWidgetIdAndPosition(int widgetId, int position);
        Task<PromptModal> GetPromptModalByWidgetIdAndPosition(int widgetId, int position);
    }
}
