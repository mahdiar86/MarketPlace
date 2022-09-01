using System;
using System.IO;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MarketPlace.Web.Controllers
{
    public class UploaderController : SiteBaseController
    {
        [HttpPost]
        [Route("Uploader/UploadImage")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر انتخاب کنید";
                var notImage = JsonConvert.DeserializeObject("{'uploaded':'0', 'error':{'message': \" " + notImageMessage + " \"} }");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            upload.AddImageToServer(fileName, FilePath.UploadImageServer, null, null);

            return Json(new { uploaded = true, url = $"{FilePath.UploadImageUrl}{fileName}" });
        }
    }
}
