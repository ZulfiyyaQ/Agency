using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels;

public class CreateCategoryVM
{
    [Required(ErrorMessage = "Name daxil etmeyiniz mutleqdir")]
    [MaxLength(50, ErrorMessage = "Uzunlugu 50 den uzun olmamalidir")]
    [MinLength(3, ErrorMessage = "Uzunlugu en azi 3 olmalidir")]
    public string Name { get; set; }

}
