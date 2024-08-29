using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class AddWidgetDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string PageName { get; set; }
    }
}
