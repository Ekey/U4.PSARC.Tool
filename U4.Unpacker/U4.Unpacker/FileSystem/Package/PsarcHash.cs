using System;
using System.Text;
using System.Security.Cryptography;

namespace U4.Unpacker
{
    class PsarcHash
    {
        public static String iGetHash(String m_String)
        {
            MD5CryptoServiceProvider TMD5 = new MD5CryptoServiceProvider();
            var lpHash = TMD5.ComputeHash(new ASCIIEncoding().GetBytes(m_String));

            return PsarcUtils.iGetStringFromBytes(lpHash);
        }
    }
}
