using System;

namespace U4.Unpacker
{
    class PsarcUtils
    {
        public static String iGetStringFromBytes(Byte[] m_Bytes)
        {
            Char[] lpBytesHex = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            Char[] lpHexString = new Char[m_Bytes.Length * 2];
            Int32 dwIndex = 0;

            foreach (Byte bByte in m_Bytes)
            {
                lpHexString[dwIndex++] = lpBytesHex[bByte >> 4];
                lpHexString[dwIndex++] = lpBytesHex[bByte & 0x0F];
            }

            return new String(lpHexString);
        }
    }
}
