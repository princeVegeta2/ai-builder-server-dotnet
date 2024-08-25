using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("projects")]
    public class Project
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
