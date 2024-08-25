using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class AddProjectDto
    {
        [Required]
        public string Name { get; set; }

    }
}
