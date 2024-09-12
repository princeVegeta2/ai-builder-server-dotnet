using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class DeleteProjectDto
    {
        [Required]
        public string Name { get; set; }
    }
}
