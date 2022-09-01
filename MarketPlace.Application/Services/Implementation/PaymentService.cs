using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        #region Create Payment Request

        public PaymentStatus CreatePaymentRequest(string merchantId, int amount, string description, string callbackUrl,
            ref string redirectUrl, string userEmail=null, string userMobile=null)
        {
            var payment = new ZarinpalSandbox.Payment(amount);
            var res = payment.PaymentRequest(description, callbackUrl, userEmail, userMobile);

            if (res.Result.Status == (int)PaymentStatus.St100)
            {
                redirectUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                return (PaymentStatus)res.Result.Status;
            }

            return (PaymentStatus)res.Status;
        }

        #endregion

        #region Payment Verification

        public PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long refId)
        {
            var payment = new ZarinpalSandbox.Payment(amount);
            var result = payment.Verification(authority).Result;
            refId = result.RefId;

            return (PaymentStatus)result.Status;
        }

        #endregion

        #region Get AuthorityCode From Callback

        public string GetAuthorityCodeFromCallback(HttpContext context)
        {
            if (context.Request.Query["Status"] == "" ||
                context.Request.Query["Status"].ToString().ToLower() != "ok" ||
                context.Request.Query["Authority"].ToString() == "")
            {
                return null;
            }

            string authority = context.Request.Query["Authority"];
            return authority.Length == 36 ? authority : null;
        }

        #endregion


    }
}
