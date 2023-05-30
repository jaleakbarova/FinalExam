using System.ComponentModel.DataAnnotations.Schema;

namespace Bilet14.Models
{
    public class OurService
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
