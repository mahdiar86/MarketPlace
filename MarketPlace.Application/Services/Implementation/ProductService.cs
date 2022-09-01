using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.DTOs.Products;
using MarketPlace.Data.Entities.Products;
using MarketPlace.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        #region Constrcutor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;
        private readonly IGenericRepository<ProductFeature> _productFeatureRepository;
        private readonly IGenericRepository<ProductGallery> _productGalleryRepository;
        private readonly IGenericRepository<TechnicalFeatures> _productTechnicalFeatureRepository;
        private readonly IGenericRepository<ProductOffer> _productOfferRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository, IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository, IGenericRepository<ProductFeature> productFeatureRepository, IGenericRepository<ProductGallery> productGalleryRepository, IGenericRepository<TechnicalFeatures> productTechnicalFeatureRepository, IGenericRepository<ProductOffer> productOfferRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSelectedCategoryRepository = productSelectedCategoryRepository;
            _productFeatureRepository = productFeatureRepository;
            _productGalleryRepository = productGalleryRepository;
            _productTechnicalFeatureRepository = productTechnicalFeatureRepository;
            _productOfferRepository = productOfferRepository;
        }

        #endregion

        #region Filter Products

        public async Task<FilterProductDTO> FilterProducts(FilterProductDTO filter)
        {
            var query = _productRepository.GetQuery().AsQueryable();
            query = query.Where(p => !p.IsDelete);

            switch (filter.FilterProductState)
            {
                case FilterProductState.Accepted:
                    query = query.Where(s => s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.UnderProgress:
                    query = query.Where(s => s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.UnderProgress);
                    break;
                case FilterProductState.Rejected:
                    query = query.Where(s => s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.Rejected);
                    break;
                case FilterProductState.All:
                    //don't change query
                    break;
            }

            switch (filter.FilterProductOrderBy)
            {
                case FilterProductOrderBy.CreateDate_DES:
                    query = query.OrderByDescending(t => t.CreateDate);
                    break;
                case FilterProductOrderBy.CreateDate_ACS:
                    query = query.OrderBy(t => t.CreateDate);
                    break;
                case FilterProductOrderBy.Price_ACS:
                    query = query.OrderBy(t => t.Price);
                    break;
                case FilterProductOrderBy.Price_DES:
                    query = query.OrderByDescending(r => r.Price);
                    break;
                default:
                    break;
            }

            if (filter.MinPrice != 0 && filter.MinPrice != null)
                query = query.Where(s => (s.Price / 100 * (s.DiscountPercent == 0 ? 100 : s.DiscountPercent)) > filter.MinPrice);

            if (filter.MaxPrice != 0 && filter.MaxPrice != null)
                query = query.Where(s => (s.Price / 100 * (s.DiscountPercent == 0 ? 100 : s.DiscountPercent)) < filter.MaxPrice);

            if (!string.IsNullOrEmpty(filter.ProductTitle))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.ProductTitle}%"));

            if (filter.SellerId != null && filter.SellerId != 0)
                query = query.Where(s => s.SellerId == filter.SellerId.Value);

            if (filter.IsExists)
                query = query.Where(p => p.QuantityInShop > 1);

            var pager = Pager.Build(filter.PageId, await query.CountAsync(),
                filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = query.ToList().PagingList(pager).ToList();

            return filter.SetProducts(allEntities).SetPaging(pager);
        }

        public async Task<List<Product>> FilterProductsForSellerByProductName(string productName, long sellerId)
        {
            return await _productRepository.GetQuery()
                .AsQueryable()
                .Where(p => p.SellerId == sellerId && EF.Functions.Like(p.Title, $"%{productName}%"))
                .ToListAsync();
        }

        #endregion

        #region Get Product Categories

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId)
        {
            if (parentId == null || parentId == 0)
                return await _productCategoryRepository.GetQuery()
                    .AsQueryable()
                    .Where(c => c.IsActive && !c.IsDelete && c.ParentId == null)
                    .OrderByDescending(d => d.LastUpdateDate)
                    .ToListAsync();

            return await _productCategoryRepository.GetQuery()
                .AsQueryable()
                .Where(c => c.IsActive && !c.IsDelete && c.ParentId == parentId)
                .OrderByDescending(d => d.LastUpdateDate)
                .ToListAsync();
        }

        public Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return _productCategoryRepository.GetQuery().AsQueryable().Where(s => s.IsActive && !s.IsDelete).ToListAsync();
        }

        #endregion

        #region Create Product

        public async Task<CreateProductResult> CreateProduct(CreateProductDTO product, long sellerId, IFormFile productImage)
        {
            if (productImage == null)
                return CreateProductResult.HasNoImage;

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);

            var res = productImage.AddImageToServer(imageName, FilePath.ProductImageServer, 300, 300, FilePath.ProductImageThumbServer);

            if (res == true)
            {
                var newProduct = new Product
                {
                    Title = product.Title,
                    Price = product.Price,
                    DiscountPercent = product.DiscountPercent,
                    IsActive = product.IsActive,
                    ProductAcceptanceState = ProductAcceptanceState.UnderProgress,
                    Visit = 1,
                    QuantityInShop = product.QuantityInShop,
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    SellerId = sellerId,
                    ImageName = imageName,
                    SiteProfit = product.SiteProfit
                };

                await _productRepository.AddEntity(newProduct);
                await _productRepository.SaveChanges();

                var productSelectedCategories = new List<ProductSelectedCategory>();

                if (product.SelectedCategories != null)
                {
                    foreach (var categoryId in product.SelectedCategories)
                    {
                        productSelectedCategories.Add(new ProductSelectedCategory
                        {
                            ProductCategoryId = categoryId,
                            ProductId = newProduct.Id
                        });
                    }
                }

                await _productSelectedCategoryRepository.AddRangeEntities(productSelectedCategories);
                await _productSelectedCategoryRepository.SaveChanges();

                var productFeatures = new List<ProductFeature>();

                if (product.ProductFeatures != null)
                {
                    foreach (var item in product.ProductFeatures)
                    {
                        productFeatures.Add(new ProductFeature
                        {
                            ProductId = newProduct.Id,
                            FeatureTitle = item.FeatureTitle,
                            FeatureValue = item.FeatureValue
                        });
                    }
                }
                await _productFeatureRepository.AddRangeEntities(productFeatures);
                await _productFeatureRepository.SaveChanges();
            }
            else
            {
                return CreateProductResult.Error;
            }

            return CreateProductResult.Success;
        }

        #endregion

        #region Get Product By Id

        public async Task<Product> GetProductById(long productId)
        {
            return await _productRepository.GetEntityById(productId);
        }


        #endregion

        #region Accept Seller Product

        public async Task<bool> AcceptSellerProduct(long id)
        {
            var product = await _productRepository.GetEntityById(id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Accepted;
                product.ProductAcceptOrRejectDescription = $"محصول مورد نظر در تاریخ {DateTime.Now.ToShamsi()} مورد تایید قرار گرفت";
                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerProduct(RejectProductDTO reject)
        {
            var product = await _productRepository.GetEntityById(reject.ProductId);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Rejected;
                product.ProductAcceptOrRejectDescription = reject.WhyIsReject;

                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();

                return true;
            }

            return false;
        }

        #endregion

        #region Edit Product
        public async Task<EditProductDTO> GetProductForEdit(long productId)
        {
            var product = await _productRepository
                .GetQuery()
                .AsQueryable()
                .Include(r => r.TechnicalFeatures)
                .SingleOrDefaultAsync(g => g.Id == productId);

            if (product == null)
                return null;

            var productFeatures = product.TechnicalFeatures
                .Select(t => new CreateProductTechnicalFeatureDTO
                {
                    FeatureTitle = t.FeatureTitle,
                    FeatureValue = t.FeatureValue,
                    ProductId = t.ProductId
                }).ToList();

            return new EditProductDTO
            {
                Id = productId,
                Description = product.Description,
                IsActive = product.IsActive,
                Price = product.Price,
                QuantityInShop = product.QuantityInShop,
                DiscountPercent = product.DiscountPercent,
                ShortDescription = product.ShortDescription,
                ImageName = product.ImageName,
                Title = product.Title,
                TechnicalFeatures = productFeatures,
                SiteProfit = product.SiteProfit,
                ProductFeatures = await _productFeatureRepository.GetQuery().AsQueryable().Where(s => !s.IsDelete && s.ProductId == productId)
                    .Select(s => new CreateProductFeatureDTO { FeatureTitle = s.FeatureTitle, FeatureValue = s.FeatureValue }).ToListAsync(),
                SelectedCategories = await _productSelectedCategoryRepository.GetQuery().AsQueryable()
                    .Where(p => p.ProductId == productId).Select(s => s.ProductCategoryId).ToListAsync()
            };
        }


        public async Task<bool> EditProduct(EditProductDTO product, IFormFile productImage)
        {
            var imageName = product.ImageName;
            var mainProduct = await _productRepository.GetEntityById(product.Id);

            if (productImage != null && productImage.IsImage())
            {
                imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
                var res = productImage.AddImageToServer(imageName, FilePath.ProductImageServer, 300, 300, FilePath.ProductImageThumbServer, mainProduct.ImageName);
            }

            var productTechnicalFeatures = new List<TechnicalFeatures>();

            await RemoveAllProductFeatures(product.Id);
            if (product.TechnicalFeatures != null && product.TechnicalFeatures.Any())
            {
                await CreateProductFeatures(product.ProductFeatures, product.Id);

                foreach (var technicalFeature in product.TechnicalFeatures)
                {
                    productTechnicalFeatures.Add(new TechnicalFeatures
                    {
                        FeatureValue = technicalFeature.FeatureValue,
                        FeatureTitle = technicalFeature.FeatureTitle,
                        ProductId = technicalFeature.ProductId
                    });
                }
            }

            var newProduct = await _productRepository.GetEntityById(product.Id);
            if (newProduct != null)
            {
                newProduct.Title = product.Title;
                newProduct.IsActive = product.IsActive;
                newProduct.ImageName = imageName;
                newProduct.DiscountPercent = product.DiscountPercent;
                newProduct.ShortDescription = product.ShortDescription;
                newProduct.Description = product.Description;
                newProduct.QuantityInShop = product.QuantityInShop;
                newProduct.Price = product.Price;
                newProduct.ProductAcceptanceState = ProductAcceptanceState.UnderProgress;
                newProduct.ProductSelectedCategories = await _productSelectedCategoryRepository.GetQuery().AsQueryable()
                    .Where(c => !c.IsDelete && c.ProductId == product.Id).ToListAsync();
                newProduct.TechnicalFeatures = productTechnicalFeatures;
                _productRepository.EditEntity(newProduct);
                await _productRepository.SaveChanges();
            }


            return true;
        }
        public async Task<Product> GetProductForAddFeature(long productId)
        {
            return await _productRepository.GetQuery().AsQueryable()
                .Include(t => t.ProductFeatures).FirstOrDefaultAsync(c => c.Id == productId && !c.IsDelete);
        }

        #endregion

        #region Delete Product

        public async Task<bool> DeleteProduct(long productId)
        {
            var product = await _productRepository.GetEntityById(productId);
            if (product != null)
            {
                _productRepository.DeleteEntity(product);
                await _productRepository.SaveChanges();

                return true;
            }

            return false;
        }

        #endregion

        #region Product Feature

        public async Task<ProductFeature> AddProductFeature(CreateProductFeatureDTO feature)
        {
            var newFeature = new ProductFeature
            {
                FeatureValue = feature.FeatureValue,
                ProductId = feature.ProductId,
                FeatureTitle = feature.FeatureTitle
            };
            await _productFeatureRepository.AddEntity(newFeature);

            await _productFeatureRepository.SaveChanges();
            return newFeature;
        }

        public async Task DeleteFeature(long featureId)
        {
            var feature = await _productFeatureRepository.GetEntityById(featureId);
            if (feature != null)
            {
                feature.IsDelete = true;
                _productFeatureRepository.EditEntity(feature);
                await _productFeatureRepository.SaveChanges();
            }

        }

        public async Task<Product> GetProductByFeatureId(long featureId)
        {
            var feature = await _productFeatureRepository.GetEntityById(featureId);
            if (feature != null)
            {
                return await GetProductById(feature.ProductId);
            }

            return null;
        }

        #endregion

        #region Product Gallery

        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            return await _productGalleryRepository.GetQuery().AsQueryable()
                .Where(g => g.ProductId == productId && !g.IsDelete).ToListAsync();
        }

        public async Task<List<ProductGallery>> GetAllProductGalleriesInSellerPanel(long productId, long sellerId)
        {
            return await _productGalleryRepository.GetQuery()
                .Include(s => s.Product)
                .OrderByDescending(t => t.DisplayPriority).Where(s => s.ProductId == productId && s.Product.SellerId == sellerId && !s.IsDelete).ToListAsync();
        }

        public async Task<Product> GetProductBySellerOwnerId(long productId, long sellerId)
        {
            return await _productRepository.GetQuery()
                .Include(s => s.Seller)
                .SingleOrDefaultAsync(p => p.Id == productId && p.Seller.UserId == sellerId);
        }

        public async Task AddProductGallery(ProductGalleryDTO gallery, IFormFile imageName)
        {
            if (imageName != null)
            {
                var filename = Guid.NewGuid().ToString("N") + "--" + imageName.FileName;
                var res = imageName.AddImageToServer(filename, FilePath.ProductGalleryImageServer, 512, 512, FilePath.ProductGalleryImageThumbServer);

                var newGallery = new ProductGallery
                {
                    ImageName = filename,
                    DisplayPriority = gallery.DisplayPriority,
                    ProductId = gallery.ProductId
                };

                await _productGalleryRepository.AddEntity(newGallery);

                await _productGalleryRepository.SaveChanges();
            }

        }

        public async Task<bool> DeleteProductGallery(long galleryId, long sellerId)
        {
            var gallery = await _productGalleryRepository.GetQuery().Include(g => g.Product)
                .FirstOrDefaultAsync(t => t.Id == galleryId);
            if (gallery != null)
            {
                if (gallery.Product.SellerId == sellerId)
                {
                    gallery.IsDelete = true;

                    _productGalleryRepository.EditEntity(gallery);
                    await _productGalleryRepository.SaveChanges();

                    return true;
                }
            }


            return false;
        }

        public async Task<long> GetProductIdByGalleryId(long galleryId)
        {
            return (await _productGalleryRepository.GetQuery()
                .FirstOrDefaultAsync(g => g.Id == galleryId)).ProductId;
        }

        #endregion

        #region Show Product Detail

        public async Task<ProductDetailDTO> GetProductDetailById(long productId)
        {
            var product = await _productRepository
                .GetQuery()
                .AsQueryable()
                .Include(t => t.Seller)
                .ThenInclude(y => y.User)
                .Include(t => t.ProductGalleries)
                .Include(w => w.TechnicalFeatures)
                .Include(r => r.ProductFeatures)
                .Include(t => t.ProductSelectedCategories)
                .ThenInclude(r => r.ProductCategory)
                .SingleOrDefaultAsync(t => t.Id == productId);
            if (product == null)
                return null;

            var selectedCategoriesIds = product.ProductSelectedCategories
                .Select(t => t.ProductCategoryId)
                .ToList();

            return new ProductDetailDTO
            {
                Price = product.Price,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                DiscountPercent = product.DiscountPercent,
                ImageName = product.ImageName,
                Seller = product.Seller,
                ProductFeatures = product.ProductFeatures,
                QuantityInShop = product.QuantityInShop,
                ProductId = product.Id,
                Visit = product.Visit,
                Title = product.Title,
                IsActive = product.IsActive,
                ProductCategories = product.ProductSelectedCategories.Select(t => t.ProductCategory).ToList(),
                ProductGalleries = product.ProductGalleries,
                ProductAcceptOrRejectDescription = product.ProductAcceptOrRejectDescription,
                TechnicalFeatures = product.TechnicalFeatures,
                RelatedProducts = await _productRepository.GetQuery()
                    .Include(q => q.ProductSelectedCategories)
                    .Where(t => t.ProductSelectedCategories.Any(f => selectedCategoriesIds.Contains(f.ProductCategoryId)) && t.Id != productId).ToListAsync()
            };
        }

        #endregion

        #region Create Product Feature

        public async Task CreateProductFeatures(List<CreateProductFeatureDTO> features, long productId)
        {
            var newFeatures = new List<TechnicalFeatures>();
            if (features != null && features.Any())
            {
                foreach (var feature in features)
                {
                    newFeatures.Add(new TechnicalFeatures
                    {
                        FeatureTitle = feature.FeatureTitle,
                        FeatureValue = feature.FeatureValue,
                        ProductId = productId
                    });
                }

                await _productTechnicalFeatureRepository.AddRangeEntities(newFeatures);
            }
        }

        #endregion

        #region Remove All Product Feature

        public async Task RemoveAllProductFeatures(long productId)
        {
            var productFeatures = await _productTechnicalFeatureRepository
                .GetQuery()
                .Where(t => t.ProductId == productId)
                .ToListAsync();

            foreach (var item in productFeatures)
            {
                _productTechnicalFeatureRepository.DeletePermanent(item);
            }

            await _productTechnicalFeatureRepository.SaveChanges();
        }

        #endregion

        #region Get Latest Products

        public async Task<List<Product>> GetLatestProducts(int count)
        {
            return await _productRepository
                .GetQuery()
                .AsQueryable()
                .OrderByDescending(t=>t.CreateDate)
                .Take(count)
                .ToListAsync();

        }

        #endregion

        #region Get All Product Offers

        public async Task<List<ProductOffer>> GetAllProductOffers()
        {
            var productOffers = await _productOfferRepository
                .GetQuery()
                .AsQueryable()
                .Include(t => t.Product)
                .Where(t => t.IsActive)
                .ToListAsync();

            if (productOffers == null)
                return new List<ProductOffer>();

            return productOffers;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
            await _productTechnicalFeatureRepository.DisposeAsync();
            await _productGalleryRepository.DisposeAsync();
            await _productFeatureRepository.DisposeAsync();
            await _productCategoryRepository.DisposeAsync();
            await _productSelectedCategoryRepository.DisposeAsync();
        }

        #endregion
    }
}
