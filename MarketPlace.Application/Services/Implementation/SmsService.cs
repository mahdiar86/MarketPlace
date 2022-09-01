using Kavenegar;
using MarketPlace.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementation
{
    public class SmsService : ISmsService
    {
        private string apikey = "3759514469616F49336E454D537830484156754B6361704547523271547A56644B4D35513479392B53346B3D";

        public async Task SendRecoveryPasswordSms(string text, string sendTo)
        {
            //Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apikey);
            //var result = await api.VerifyLookup("09153152743", "123456", "MarketPlace");

            var sender = "1000596446";
            var receptor = sendTo;
            var message = "فروشگاه ساز \n" + " رمز عبور جدید شما : " + text;
            var api = new KavenegarApi(apikey);
            await api.Send(sender, receptor, message);
        }

        public async Task SendVerificationSms(string text, string sendTo)
        {
            //Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apikey);
            //var result = await api.VerifyLookup("09153152743", "123456", "MarketPlace");

            var sender = "1000596446";
            var receptor = sendTo;
            var message = "فروشگاه ساز \n" + " کد تایید حساب کاربری شما : " + text;
            var api = new KavenegarApi(apikey);
            await api.Send(sender, receptor, message);
        }
    }
}
