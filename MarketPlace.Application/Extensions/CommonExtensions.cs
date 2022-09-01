using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MarketPlace.Application.Extensions
{
    public static class CommonExtensions
    {
        public static string GetEnumName(this Enum myEnum)
        {
            var enumDisplayName = myEnum.GetType()
                .GetMember(myEnum.ToString())
                .FirstOrDefault();

            if (enumDisplayName != null)
            {
                return enumDisplayName.GetCustomAttribute<DisplayAttribute>()?.GetName();
            }

            return "";
        }
    }
}
