using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.Data.DTOs.Products;
using MarketPlace.Web.Http;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region Constrcutor

        private IProductService _productService;
        private readonly ISellerService _sellerService;
        private IUserService _userService;

        public ProductController(IProductService productService, ISellerService sellerService, IUserService userService)
        {
            _productService = productService;
            _sellerService = sellerService;
            _userService = userService;
        }

        #endregion 

        #region Products

        [HttpGet("Products")]
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            filter.SellerId = seller.Id;
            filter.FilterProductState = FilterProductState.All;

            return View(await _productService.FilterProducts(filter));
        }

        [HttpGet("Create-Product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.mainCategories = await _productService.GetAllActiveProductCategories();

            return View();
        }

        [HttpPost("Create-Product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var result = await _productService.CreateProduct(product, seller.Id, productImage);

                switch (result)
                {
                    case CreateProductResult.HasNoImage:
                        TempData[ErrorMessage] = "لطفا تصویری برای محصول خود انتخاب کنید";
                        break;
                    case CreateProductResult.HasNoDescription:
                        TempData[ErrorMessage] = "لطفا توضیحات محصول را وارد کنید";
                        break;
                    case CreateProductResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت محصول با خطا مواجه شد !";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = $"محصول {product.Title} با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }

            ViewBag.mainCategories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }

        [HttpGet("Edit-Product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);

            if (product == null)
                return NotFound();


            ViewBag.mainCategories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }

        [HttpPost("Edit-Product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDTO product, IFormFile productImage)
        {
            if (product == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _productService.EditProduct(product, productImage);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.mainCategories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(long productId)
        {
            if (await _productService.DeleteProduct(productId))
            {
                return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "محصول مورد نظر با موفقیت حذف شد", null);
            }

            return JsonResponseResult.SendStatus(JsonResultStatusType.Error, "حذف محصول با خطا روبرو شده است", null);
        }

        #endregion

        #region Product Features

        [HttpGet("AddProductFeature/{productId}")]
        public async Task<IActionResult> AddProductFeature(long productId)
        {
            var product = await _productService.GetProductForAddFeature(productId);
            if (product == null)
                return NotFound();

            ViewBag.product = product;

            return View();
        }

        [HttpPost("AddProductFeature")]
        public async Task<dynamic> AddProductFeature(string featureTitle, string featureValue, int productId)
        {
            var feature = new CreateProductFeatureDTO()
            {
                FeatureValue = featureValue,
                FeatureTitle = featureTitle,
                ProductId = productId
            };
            var res = await _productService.AddProductFeature(feature);
            return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "", null);
        }

        [HttpGet("DeleteProductFeature/{featureId}")]
        public async Task<IActionResult> DeleteProductFeature(int featureId)
        {
            var product = await _productService.GetProductByFeatureId(featureId);
            if (product == null)
                return NotFound();

            await _productService.DeleteFeature(featureId);

            return Redirect("/Seller/AddProductFeature/" + product.Id);
        }

        #endregion

        #region Product Galleries

        [HttpGet("Product-Galleries/{productId}")]
        public async Task<IActionResult> GetProductGalleries(long productId)
        {
            ViewBag.product = await _productService.GetProductById(productId);

            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            return View(await _productService.GetAllProductGalleriesInSellerPanel(productId, seller.Id));
        }

        [HttpPost("Create-Product-Gallery")]
        public async Task<IActionResult> CreateProductGallery(ProductGalleryDTO gallery, IFormFile imageName)
        {
            if (imageName != null && imageName.IsImage())
            {
                if (imageName.Length < 4194304)
                {
                    var product = await _productService.GetProductBySellerOwnerId(gallery.ProductId, User.GetUserId());
                    if (product == null)
                        return JsonResponseResult.SendStatus(JsonResultStatusType.Error, "شما دسترسی ویرایش این محصول را ندارید !", null);

                    await _productService.AddProductGallery(gallery, imageName);

                    return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "تصویر مورد نظر با موفقیت اضافه شد", null);
                }
                else
                {
                    return JsonResponseResult.SendStatus(JsonResultStatusType.Warning, "تصویر مورد نظر باید زیر 4 مگابایت باشد", null);
                }
            }

            return JsonResponseResult.SendStatus(JsonResultStatusType.Error, "لطفا تصویر مناسبی انتخاب کنید !", null);
        }

        [HttpGet("Product-Galleries/Delete-Gallery/{id}")]
        public async Task<IActionResult> DeleteProductGallery(long id)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            bool result = await _productService.DeleteProductGallery(id, seller.Id);

            if (result)
                TempData[SuccessMessage] = "تصویر مورد نظر از گالری محصول حذف شد !";
            else
                TempData[ErrorMessage] = "خطایی رخ داده است";


            return Redirect("/Seller/Product-Galleries/" + await _productService.GetProductIdByGalleryId(id));
        }

        #endregion

        #region Get Products Json

        [HttpGet("products-autocomplete")]
        public async Task<IActionResult> GetSellerProductsJson(string productName)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            var data = await _productService.FilterProductsForSellerByProductName(productName, seller.Id);

            return new JsonResult(data);
        }

        #endregion
    }
}
