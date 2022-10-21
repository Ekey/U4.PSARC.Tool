using System;
using System.IO;

namespace U4.Unpacker
{
    class PsarcChunks
    {
        //http://aluigi.org/bms/brink.bms

        public static Byte[] iDecompressCore(Byte[] lpBuffer, UInt16 wMagic, Int32 dwCompressedSize, Int32 dwDecompressedSize)
        {
            if (wMagic == 0x68C)
            {
                var lpDstBuffer = Oodle.iDecompress(lpBuffer, dwCompressedSize, (Int32)dwDecompressedSize);
                return lpDstBuffer;
            }
            else if (wMagic == 0xDA78)
            {
                var lpDstBuffer = Zlib.iDecompress(lpBuffer);
                return lpDstBuffer;
            }

            return lpBuffer;
        }

        public static Byte[] iReadData(FileStream TPsarStream, PsarcHeader m_Header, PsarcEntry m_Entry, Int32[] m_LengthsTable)
        {
            Int32 dwOffset = 0;
            Int32 dwIndex = m_Entry.dwEntryIndex;

            Int32 dwChunkSize = 65536;
            Int64 dwBlockOffset = m_Entry.dwOffset;
            Int32 dwRemainedSize = (Int32)m_Entry.dwDecompressedSize;

            Byte[] lpResult = new Byte[m_Entry.dwDecompressedSize];

            TPsarStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
            UInt16 wMagic = TPsarStream.ReadUInt16();

            if (wMagic == 0x68C || wMagic == 0xDA78)
            {
                do
                {
                    TPsarStream.Seek(dwBlockOffset, SeekOrigin.Begin);

                    Int32 dwCompressedChunkSize = m_LengthsTable[dwIndex];
                    if (dwCompressedChunkSize == 0)
                    {
                        dwCompressedChunkSize = dwChunkSize;
                    }

                    if (dwRemainedSize < dwChunkSize || dwCompressedChunkSize == dwChunkSize)
                    {
                        var lpSrcBuffer = TPsarStream.ReadBytes(dwCompressedChunkSize);
                        var lpDstBuffer = iDecompressCore(lpSrcBuffer, wMagic, dwCompressedChunkSize, (Int32)dwRemainedSize);

                        Array.Copy(lpDstBuffer, 0, lpResult, dwOffset, lpDstBuffer.Length);
                        dwOffset += lpDstBuffer.Length;
                    }
                    else
                    {
                        var lpSrcBuffer = TPsarStream.ReadBytes(dwCompressedChunkSize);
                        var lpDstBuffer = iDecompressCore(lpSrcBuffer, wMagic, dwCompressedChunkSize, (Int32)dwChunkSize);

                        Array.Copy(lpDstBuffer, 0, lpResult, dwOffset, lpDstBuffer.Length);
                        dwOffset += lpDstBuffer.Length;
                    }

                    dwIndex++;
                    dwBlockOffset += dwCompressedChunkSize;
                    dwRemainedSize -= dwChunkSize;
                }
                while (dwOffset < m_Entry.dwDecompressedSize);
            }
            else
            {
                TPsarStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                var lpBuffer = TPsarStream.ReadBytes((Int32)m_Entry.dwDecompressedSize);
                
                return lpBuffer;
            }

            dwIndex++;

            return lpResult;
        }
    }
}
