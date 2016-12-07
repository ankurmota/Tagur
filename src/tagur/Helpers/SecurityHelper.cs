using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Helpers
{
    public static class SecurityHelper
    {
        public static string AsCredential(this string value)
        {
            return value + $";Initial Catalog={SecretHelper.SecureDatabase};User ID={SecretHelper.SecureUserId};Password={SecretHelper.SecurePassword}";
        }
    }
}
