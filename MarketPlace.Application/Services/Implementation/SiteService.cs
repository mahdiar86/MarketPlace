using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.Entities;
using MarketPlace.Data.Entities.SiteSettings;
using MarketPlace.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementation
{
    public class SiteService : ISiteService
    {
        #region constrcutor

        private readonly IGenericRepository<Site> _siteSettingRepository;
        private readonly IGenericRepository<Slider> _sliderRepository;
        private readonly IGenericRepository<SiteBanner> _bannerRepository;

        public SiteService(IGenericRepository<Site> siteSettingRepository, IGenericRepository<Slider> sliderRepository,IGenericRepository<SiteBanner> bannerRepository)
        {
            _siteSettingRepository = siteSettingRepository;
            _sliderRepository = sliderRepository;
            _bannerRepository = bannerRepository;
        }

        #endregion

        public async Task<Site> GetDefaultSiteSettings()
        {
            return await _siteSettingRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(d => d.IsDefault && !d.IsDelete);
        }

        public async Task<List<Slider>> GetAllActiveSliders()
        {
            return await _sliderRepository.GetQuery()
                .AsQueryable()
                .Where(s => s.IsActive && !s.IsDelete).ToListAsync();
        }

        public async Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements)
        {
            return await _bannerRepository.GetQuery().AsQueryable()
                .Where(s => placements.Contains(s.BannerPlacement)).ToListAsync();
        }

        #region dispose

        public async ValueTask DisposeAsync()
        {
            if (_siteSettingRepository != null)
                await _siteSettingRepository.DisposeAsync();

            if (_sliderRepository != null)
                await _sliderRepository.DisposeAsync();

            if (_bannerRepository != null)
                await _bannerRepository.DisposeAsync();
        }

        #endregion
    }
}
