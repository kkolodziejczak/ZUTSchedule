using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ZUTSchedule.core
{
    public static class StringSecureHelper
    {
        /// <summary>
        /// Unsecures <see cref="SecureString"/> to plain text
        /// </summary>
        /// <param name="secureString"></param>
        /// <returns></returns>
        public static string Unsecure(this SecureString secureString)
        {
            if(secureString == null)
            {
                return string.Empty;
            }

            var unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = SecureStringMarshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
