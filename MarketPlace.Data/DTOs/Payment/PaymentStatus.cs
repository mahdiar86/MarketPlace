using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Payment
{
    public enum PaymentStatus
    {
        [Display(Name = "اطلاعات ارسالی ناقص می باشد")]
        St1 = -1,
        [Display(Name = "باتوجه به محدودیت های شاپرک امکان پرداخت با رقم درخواستی میسر نمی باشد")]
        St3 = -3,
        [Display(Name = "درخواست موردنظر یافت نشد")]
        St11 = -11,
        [Display(Name = "هیچ نوع عملیات مالی برای این تراکنش یافت نشد")]
        St21 = -21,
        [Display(Name = "رقم تراکنش با رقم پرداخت شده مطابقت ندارد")]
        St33 = -33,
        [Display(Name = "عملیات با موفقیت انجام گردیده است")]
        St100 = 100,
        [Display(Name = "عملیات پرداخت موفق بوده و قبلا Payment Verification انجام شده است")]
        St101 =  101
    }
}
