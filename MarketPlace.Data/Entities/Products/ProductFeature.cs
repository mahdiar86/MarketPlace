using System.ComponentModel.DataAnnotations;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.Products
{
    public class ProductFeature : BaseEntity
    {
        public long ProductId { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150 , ErrorMessage = "")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "")]
        public string FeatureValue { get; set; }

        public Product Product { get; set; }
    }
}