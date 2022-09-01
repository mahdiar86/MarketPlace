using MarketPlace.Data.Entities.SiteSettings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        Task<Site> GetDefaultSiteSettings();
        Task<List<Slider>> GetAllActiveSliders();
        Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements);
    }
}
