using Agency.Models;
using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        [Required]
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        public string ImageUrl { get; set; }
    }
}
