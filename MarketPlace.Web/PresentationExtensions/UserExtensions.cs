using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Account;

namespace MarketPlace.Web.PresentationExtensions
{
    public static class UserExtensions
    {
        public static string GetUserShowName(this User user)
        {
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }

            return user.Mobile;
        }
    }
}
