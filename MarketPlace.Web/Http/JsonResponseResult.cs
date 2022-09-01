using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Http
{
    public static class JsonResponseResult
    {
        public static JsonResult SendStatus(JsonResultStatusType type, string message, object data)
        {
            return new JsonResult(new { status = type.ToString(), message = message, data = data });
        }
    }

    public enum JsonResultStatusType
    {
        Success, Warning, Info, Error
    }
}
