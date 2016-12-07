using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tagur.Helpers
{
    public static class SecretHelper
    {
        public static string SecureDatabase
        {
            get
            {
                return "[YOUR SQL SERVER DATABASE NAME]";
            }
        }

        public static string SecurePassword
        {
            get
            {
                return "[YOUR SQL SERVER PASSWORD]";
            }
        }

        public static string SecureUserId
        {
            get
            {
                return "[YOUR SQL SERVER USER NAME]";
            }
        }
    }
}
