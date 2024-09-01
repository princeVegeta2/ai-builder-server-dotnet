using AIBuilderServerDotnet.Models;
using System.Security.Cryptography.X509Certificates;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IModalValuesRepository
    {
        // Adding values to "colors", "links", "image_links", and "prompts" tables
        Task AddColorValue(Color color);
        Task AddLinkValue(Link link);
        Task AddImageLinkValue(ImageLink imageLink);
        Task AddPromptValue(Prompt prompt);

        // Getting values from "colors", "links", "image_links", and "prompts" tables
        Task<Color> GetColorByColorModalIdAndPosition(int colorModalId, int position);
        Task<Link> GetLinkByLinkModalIdAndPosition(int linkModalId, int position);
        Task<ImageLink> GetImageLinkByImageLinkModalIdAndPosition(int imageLinkModalId, int position);
        Task<Prompt> GetPromptByPromptModalIdAndPosition(int promptModalId, int position);

        // Checking if values exist for modals in the values tables
        Task<bool> CheckIfColorValueExistsForAModal(string colorValue, int colorModalId);
        Task<bool> CheckIfLinkNameExistsForAModal(string linkName, int linkModalId);
        Task<bool> CheckIfUrlExistsForAModal(string url, int linkModalId);
        Task<bool> CheckIfImageImageUrlExistsForAModal(string imageUrl, int linkModalId);
        Task<bool> CheckIfPromptExistsForAModal(string prompt, int promptModalId);

        // Checking if position already exists for in a value table tied to modal id
        Task<bool> PositionAlreadyExistsForColorModalId(int position, int modalId);
        Task<bool> PositionAlreadyExistsForLinkModalId(int position, int modalId);
        Task<bool> PositionAlreadyExistsForImageLinkModalId(int position, int modalId);
        Task<bool> PositionAlreadyExistsForPrompModalId(int position, int modalId);

        // Updating values in "colors", "links", "image_links", and "prompts" tables
        Task UpdateColorValue(string colorValue, int colorModalId, int position);
        Task UpdateLinkName(string linkName, int linkModalId, int position);
        Task UpdateLinkUrl(string linkUrl, int linkModalId, int position);
        Task UpdateImageLinkUrl(string imageLinkUrl, int imageLinkModalId, int position);
        Task UpdatePromptValue(string prompt, int promptModalId, int position);

        // Deleting instances in "colors", "links", "image_links" and "prompts" tables
        Task DeleteColor(Color color);
        Task DeleteLink(Link link);
        Task DeleteImageLink(ImageLink imageLink);
        Task DeletePrompt(Prompt prompt);
    }
}
