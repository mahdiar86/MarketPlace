using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.Data.Entities.SiteSettings;
using MarketPlace.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementation
{
    public class NewsletterService : INewletterService
    {
        #region Constructor

        private readonly IGenericRepository<Newsletter> _newsletterRepository;

        public NewsletterService(IGenericRepository<Newsletter> newsletterRepository)
        {
            _newsletterRepository = newsletterRepository;
        }

        #endregion

        #region Add Email To Newsletter

        public async Task<bool> AddEmailToNewsletter(string email)
        {
            bool isExist = await _newsletterRepository.GetQuery().AsQueryable().AnyAsync(t => t.Email == email.FixedEmail());

            if (isExist)
                return false;

            await _newsletterRepository.AddEntity(new Newsletter { Email = email.FixedEmail() });
            await _newsletterRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            await _newsletterRepository.DisposeAsync();
        }

        #endregion
    }
}
