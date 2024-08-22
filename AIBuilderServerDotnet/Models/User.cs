using System;

namespace AIBuilderServerDotnet.Models
{
    public class User
    {
        public int Id { get; set; } // Corresponds to id SERIAL PRIMARY KEY
        public string Username { get; set; } // Corresponds to username VARCHAR(100) NOT NULL
        public string Email { get; set; } // Corresponds to email VARCHAR(100) NOT NULL UNIQUE
        public string PasswordHash { get; set; } // Corresponds to password_hash VARCHAR(255) NOT NULL
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Corresponds to created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        public bool BuilderAccess { get; set; } = false; // Corresponds to builder_access BOOLEAN DEFAULT false
    }
}
