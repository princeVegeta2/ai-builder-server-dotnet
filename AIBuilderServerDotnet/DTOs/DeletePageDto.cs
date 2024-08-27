using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class DeletePageDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ProjectName { get; set; }
    }
}