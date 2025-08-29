using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApi.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; } // Auto-increment PK

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; } = null!;
    }
}