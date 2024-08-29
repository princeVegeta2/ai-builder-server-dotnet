using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IWidgetRepository
    {
        Task AddWidget(Widget widget);

        Task DeleteWidget(Widget widget);

        Task UpdateWidgetPositionsForAPage(int pageId, int widgetPosition);

        Task<Widget> GetWidgetByPageIdAndPosition(int pageId, int widgetPosition);
    }
}
