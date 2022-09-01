using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Products
{
    public class ProductGalleryDTO
    {
        public int DisplayPriority { get; set; }

        public long ProductId { get; set; }
    }
}
