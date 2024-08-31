using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("colors")]
    public class Color
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("color_modal_id")]
        public int ColorModalId { get; set; }
        [Column("color")]
        public string ColorValue { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
