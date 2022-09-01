using System.IO;

namespace MarketPlace.Application.Utils
{
    public static class FilePath
    {
        public static string SliderOrigin = "/images/main-slider/";
        public static string BannerOrigin = "/images" +
            "/banner/";

        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/Origin/";
        public static string UserAvatarServerOrigin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Origin/");
        public static string DefaultAvatar = "/Content/Defaults/avatar.png";

        public static string UserAvatarThumb = "/Content/Images/UserAvatar/Thumbs/";
        public static string UserAvatarServerThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumbs/");

        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Uploads/");
        public static string UploadImageUrl = "/Content/Uploads/";

        public static string UploadCertificateServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Certificates/");
        public static string UploadCertificateOrigin = "/Content/Certificates/";


        public static string ProductImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Products/Origin/");
        public static string ProductImageOrigin = "/Content/Products/Origin/";

        public static string ProductImageThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Products/Thumb/");
        public static string ProductImageThumb = "/Content/Products/Thumb/";
        
        public static string ProductGalleryImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/ProductGalleries/Origin/");
        public static string ProductGalleryImageOrigin = "/Content/ProductGalleries/Origin/";

        public static string ProductGalleryImageThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/ProductGalleries/Thumb/");
        public static string ProductGalleryImageThumb = "/Content/ProductGalleries/Thumb/";


        public static string DomainAddress = "http://localhost:16985";
    }
}
