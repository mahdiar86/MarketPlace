using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.ProductOrder
{
    public class Order : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }

        [Display(Name = "کد پیگیری")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TracingCode { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool IsPaid { get; set; }

        #endregion

        #region Relations

        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
