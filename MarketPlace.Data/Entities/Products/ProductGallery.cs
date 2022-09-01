using System.ComponentModel.DataAnnotations;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.Products
{
    public class ProductGallery : BaseEntity
    {
        #region Properties

        [Display(Name = "نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        public int DisplayPriority { get; set; }

        public long ProductId { get; set; }


        #endregion
        
        #region Relations

        public Product Product { get; set; }

        #endregion

    }
}
