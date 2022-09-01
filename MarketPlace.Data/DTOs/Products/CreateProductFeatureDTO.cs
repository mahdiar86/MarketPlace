using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Data.DTOs.Products
{
    public class CreateProductFeatureDTO
    {
        public string FeatureTitle { get; set; }
        public string FeatureValue { get; set; }

        public int ProductId { get; set; }
    }
}