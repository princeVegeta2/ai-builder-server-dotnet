namespace AIBuilderServerDotnet.DTOs
{
    public class PageDto
    {
        public string PageName { get; set; }
        public int Position { get; set; }
        public List<WidgetDto> Widgets { get; set; }
    }
}
