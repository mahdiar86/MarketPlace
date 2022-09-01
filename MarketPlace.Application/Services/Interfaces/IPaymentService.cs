using MarketPlace.Data.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        PaymentStatus CreatePaymentRequest(string merchantId, int amount, string description, string callBackUrl, ref string redirectUrl, string userEmail, string userMobile);

        PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long refId);

        string GetAuthorityCodeFromCallback(HttpContext context);
    }
}
