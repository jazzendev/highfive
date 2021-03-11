using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Utility
{
    public static class StringHelper
    {
        public static string DecodeBase64(this string hash)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(hash);
                var password = Encoding.UTF8.GetString(base64EncodedBytes);

                return password;
            }
            catch
            {
                return null;
            }
        }
    }
}
