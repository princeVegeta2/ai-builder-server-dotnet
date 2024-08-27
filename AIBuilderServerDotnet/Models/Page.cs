using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("pages")]
    public class Page
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("project_id")]
        public int ProjectId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("position")]
        public int Position { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Ensure CreatedAt is always in UTC
        public void SetCreatedAtToUtc()
        {
            CreatedAt = DateTime.SpecifyKind(CreatedAt, DateTimeKind.Utc);
        }
    }
}
