using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class UpdatePageDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string NewName { get; set; }
    }
}
