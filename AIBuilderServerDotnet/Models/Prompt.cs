using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("prompts")]
    public class Prompt
    {
        [Column("id)")]
        public int Id { get; set; }
        [Column("prompt_modal_id")]
        public int PromptModalId { get; set; }
        [Column("prompt")]
        public string PromptValue { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
