namespace MarketPlace.Application.Services.Interfaces
{
    public interface IPasswordHelper
    {
        string EncodePasswordMd5(string pass);
    }
}
