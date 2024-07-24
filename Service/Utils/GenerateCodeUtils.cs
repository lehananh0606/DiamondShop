using ShopRepository.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utils
{
    public static class GenerateCodeUtils
    {
        public static string GenerateResource4Wallet(long walletId, string productCode)
        {
            var currentTime = DateTime.Now;
            var formattedTime = currentTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var resource = $"{productCode}-{walletId}-{formattedTime}";

            return resource;
        }

        public static string GenerateCode4Transaction(TypeTrans type, string context, long userId)
        {
            var currentTime = DateTime.Now;
            var formattedTime = currentTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var resource = $"{type}-{formattedTime}-{context}-{userId}";

            return resource;
        }
    }
}