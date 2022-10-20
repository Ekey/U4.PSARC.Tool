using System;
using System.IO;

namespace U4.Unpacker
{
    class PsarcChunks
    {
        public static Byte[] iDecompress(FileStream TPsarStream, PsarcEntry m_Entry, Int32[] m_LengthsTable)
        {
            Int32 dwOffset = 0;
            Int32 dwIndex = m_Entry.dwEntryIndex;

            UInt32 dwLeftSize = (UInt32)m_Entry.dwDecompressedSize;
            Byte[] lpResult = new Byte[m_Entry.dwDecompressedSize];

            TPsarStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

            do
            {
                UInt32 dwDecompressedChunkSize = 65536;
                Int32 dwCompressedChunkSize = m_LengthsTable[dwIndex];

                if (dwCompressedChunkSize == m_Entry.dwDecompressedSize)
                {
                    var lpBuffer = TPsarStream.ReadBytes(m_Entry.dwDecompressedSize);

                    Array.Copy(lpBuffer, 0, lpResult, dwOffset, lpBuffer.Length);
                    dwOffset += lpBuffer.Length;
                }
                else
                {
                    if (dwLeftSize < dwDecompressedChunkSize)
                    {
                        dwDecompressedChunkSize = dwLeftSize;
                    }
                    else
                    {
                        dwLeftSize -= dwDecompressedChunkSize;
                    }

                    var lpSrcBuffer = TPsarStream.ReadBytes(dwCompressedChunkSize);
                    var lpDstBuffer = Oodle.iDecompress(lpSrcBuffer, dwCompressedChunkSize, (Int32)dwDecompressedChunkSize);

                    Array.Copy(lpDstBuffer, 0, lpResult, dwOffset, lpDstBuffer.Length);
                    dwOffset += lpDstBuffer.Length;
                }

                dwIndex++;
            }
            while (dwOffset < m_Entry.dwDecompressedSize);

            return lpResult;
        }
    }
}
