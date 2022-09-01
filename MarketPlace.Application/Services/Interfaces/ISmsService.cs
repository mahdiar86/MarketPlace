using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISmsService
    {
        Task SendVerificationSms(string text, string sendTo);
        Task SendRecoveryPasswordSms(string text, string sendTo);
    }
}
