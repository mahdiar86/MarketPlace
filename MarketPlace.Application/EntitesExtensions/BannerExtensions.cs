using MarketPlace.Application.Utils;
using MarketPlace.Data.Entities.SiteSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.EntitesExtensions
{
    public static class BannerExtensions
    {
        public static string GetBannerMainImageAddress(this SiteBanner banner)
        {
            return FilePath.BannerOrigin + banner.ImageName;
        }
    }
}
