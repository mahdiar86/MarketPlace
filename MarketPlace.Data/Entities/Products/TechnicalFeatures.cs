using System.ComponentModel.DataAnnotations;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.Products
{
    public class TechnicalFeatures : BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        [Display(Name = "عنوان مشخصه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار مشخصه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureValue { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }

        #endregion
    }
}
