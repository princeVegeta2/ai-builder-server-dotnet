using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IModalRepository
    {
        Task AddColorModal(ColorModal colorModal);
        Task AddLinkModal(LinkModal linksModal);
        Task AddImageLinkModal(ImageLinkModal imageLinkModal);
        Task AddPromptModal(PromptModal promptModal);
    }
}
