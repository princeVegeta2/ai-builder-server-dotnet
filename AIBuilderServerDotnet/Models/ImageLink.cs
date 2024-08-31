using System.ComponentModel.DataAnnotations.Schema;

namespace AIBuilderServerDotnet.Models
{
    [Table("image_links")]
    public class ImageLink
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("image_link_modal_id")]
        public int ImageLinkModalId { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}
