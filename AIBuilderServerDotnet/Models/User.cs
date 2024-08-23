using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; } // Corresponds to id SERIAL PRIMARY KEY

        [Column("username")]
        public string Username { get; set; } // Corresponds to username VARCHAR(100) NOT NULL

        [Column("email")]
        public string Email { get; set; } // Corresponds to email VARCHAR(100) NOT NULL UNIQUE

        [Column("password_hash")]
        public string PasswordHash { get; set; } // Corresponds to password_hash VARCHAR(255) NOT NULL

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Corresponds to created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP

        [Column("builder_access")]
        public bool BuilderAccess { get; set; } = false; // Corresponds to builder_access BOOLEAN DEFAULT false
    }
}
