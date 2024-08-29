using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("color_modals")]
    public class ColorModal
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("widget_id")]
        public int WidgetId { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
