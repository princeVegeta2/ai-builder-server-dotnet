namespace AIBuilderServerDotnet.DTOs
{
    public class WidgetDto
    {
        public string Type { get; set; }
        public int Position { get; set; }
        public ColorModalDto ColorModal { get; set; }
        public LinkModalDto LinkModal { get; set; }
        public ImageLinkModalDto ImageLinkModal { get; set; }
        public PromptModalDto PromptModal { get; set; }
    }
}
