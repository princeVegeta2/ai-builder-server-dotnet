using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class DeleteWidgetDto
    {
        [Required]
        public int Position { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string PageName { get; set; }
    }
}
