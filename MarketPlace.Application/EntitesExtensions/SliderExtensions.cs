using MarketPlace.Application.Utils;
using MarketPlace.Data.Entities.SiteSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.EntitesExtensions
{
    public static class SliderExtensions
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return FilePath.SliderOrigin + slider.ImageName;
        }
    }
}
