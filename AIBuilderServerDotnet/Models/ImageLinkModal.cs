﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("image_link_modals")]
    public class ImageLinkModal
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("widget_id")]
        public int WidgetId { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
