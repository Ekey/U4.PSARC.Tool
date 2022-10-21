using System;
using System.IO;
using System.Collections.Generic;

namespace U4.Unpacker
{
    class PsarHashList
    {
        private static Dictionary<String, String> m_HashList = new Dictionary<String, String>();

        public static void iLoadNames(Byte[] lpBuffer)
        {
            using (var TNamesReader = new MemoryStream(lpBuffer))
            {
                String m_Line = null;
                do
                {
                    m_Line = TNamesReader.ReadString();
                    String dwHash = PsarcHash.iGetHash(m_Line);
                    m_HashList.Add(dwHash, m_Line);
                }
                while (TNamesReader.Position != lpBuffer.Length);

                TNamesReader.Dispose();
            }
        }

        public static String iGetNameFromHashList(String m_Hash)
        {
            String m_FileName = null;

            if (m_Hash == "00000000000000000000000000000000")
            {
                m_FileName = "NamesLookUp.txt";
                return m_FileName;
            }

            if (m_HashList.ContainsKey(m_Hash))
            {
                m_HashList.TryGetValue(m_Hash, out m_FileName);
            }
            else
            {
                m_FileName = @"__Unknown\" + m_Hash;
            }

            return m_FileName;
        }
    }
}
