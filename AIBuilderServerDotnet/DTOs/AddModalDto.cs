using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class AddModalDto
    {
        [Required]
        public int Position { get; set; }

        [Required]
        public int WidgetPosition { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string PageName { get; set; }

        [Required]
        public string Type { get; set; }

    }
}
