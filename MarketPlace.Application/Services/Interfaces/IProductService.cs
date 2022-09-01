using MarketPlace.Data.DTOs.Products;
using MarketPlace.Data.Entities.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        Task<FilterProductDTO> FilterProducts(FilterProductDTO filter);
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategories();
        Task<CreateProductResult> CreateProduct(CreateProductDTO product, long sellerId, IFormFile productImage);
        Task<Product> GetProductById(long productId);
        Task<bool> AcceptSellerProduct(long id);
        Task<bool> RejectSellerProduct(RejectProductDTO reject);
        Task<EditProductDTO> GetProductForEdit(long productId);
        Task<ProductFeature> AddProductFeature(CreateProductFeatureDTO feature);
        Task<bool> EditProduct(EditProductDTO product, IFormFile productImage);
        Task<Product> GetProductForAddFeature(long productId);
        Task DeleteFeature(long featureId);
        Task<Product> GetProductByFeatureId(long featureId);
        Task<bool> DeleteProduct(long productId);
        Task<List<ProductGallery>> GetAllProductGalleries(long productId);
        Task<List<ProductGallery>> GetAllProductGalleriesInSellerPanel(long productId, long sellerId);
        Task<Product> GetProductBySellerOwnerId(long sellerId, long productId);
        Task AddProductGallery(ProductGalleryDTO gallery,IFormFile imageName);
        Task<bool> DeleteProductGallery(long galleryId,long sellerId);
        Task<long> GetProductIdByGalleryId(long galleryId);
        Task<ProductDetailDTO> GetProductDetailById(long productId);
        Task CreateProductFeatures(List<CreateProductFeatureDTO> features, long productId);
        Task RemoveAllProductFeatures(long productId);
        Task<List<Product>> FilterProductsForSellerByProductName(string productName, long sellerId);
        Task<List<Product>> GetLatestProducts(int count);
        Task<List<ProductOffer>> GetAllProductOffers();
    }
}