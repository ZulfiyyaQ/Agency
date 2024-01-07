using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agency.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title daxil etmeyiniz mutleqdir")]
        [MaxLength(50, ErrorMessage = "Uzunlugu 50 den uzun olmamalidir")]
        [MinLength(3, ErrorMessage = "Uzunlugu en azi 3 olmalidir")]
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
