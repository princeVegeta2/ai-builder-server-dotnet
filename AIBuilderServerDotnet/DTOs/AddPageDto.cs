using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class AddPageDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public string ProjectName { get; set; }
    }
}
