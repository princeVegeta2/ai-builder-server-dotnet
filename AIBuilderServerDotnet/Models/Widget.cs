﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("widgets")]
    public class Widget
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("page_id")]
        public int PageId { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("created-at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}