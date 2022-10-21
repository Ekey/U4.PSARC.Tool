using System;
using System.IO;
using System.Collections.Generic;

namespace U4.Unpacker
{
    class PsarcUnpack
    {
        static List<PsarcEntry> m_EntryTable = new List<PsarcEntry>();
        static List<String> m_NamesLookUp = new List<String>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TPsarStream = File.OpenRead(m_Archive))
            {
                var m_Header = new PsarcHeader();

                m_Header.dwMagic = TPsarStream.ReadUInt32();

                if (m_Header.dwMagic != 0x52415350)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of PSAR archive file!");
                    return;
                }

                m_Header.wMajorVersion = TPsarStream.ReadInt16(true);
                m_Header.wMinorVersion = TPsarStream.ReadInt16(true);

                if (m_Header.wMajorVersion != 1 || m_Header.wMinorVersion != 4)
                {
                    Utils.iSetError("[ERROR]: Invalid version of PSAR archive file!");
                    return;
                }

                m_Header.dwCompressionType = TPsarStream.ReadUInt32();

                if ((PsarcCompTypes)m_Header.dwCompressionType != PsarcCompTypes.ZLIB && (PsarcCompTypes)m_Header.dwCompressionType != PsarcCompTypes.OODLE)
                {
                    Utils.iSetError("[ERROR]: Invalid compression type of PSAR archive file!");
                    return;
                }

                m_Header.dwHeaderSize = TPsarStream.ReadInt32(true);
                m_Header.dwEntrySize = TPsarStream.ReadInt32(true);
                m_Header.dwTotalFiles = TPsarStream.ReadInt32(true);
                m_Header.dwBlockSize = TPsarStream.ReadInt32(true);
                m_Header.dwArchiveFlags = TPsarStream.ReadInt32(true);

                m_EntryTable.Clear();
                m_NamesLookUp.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new PsarcEntry();

                    m_Entry.m_NameHash = PsarcUtils.iGetStringFromBytes(TPsarStream.ReadBytes(16));
                    m_Entry.dwEntryIndex = TPsarStream.ReadInt32(true);
                    m_Entry.dwDecompressedSize = ((Int64)TPsarStream.ReadByte()) << 32 | TPsarStream.ReadUInt32(true);
                    m_Entry.dwOffset = ((Int64)TPsarStream.ReadByte()) << 32 | TPsarStream.ReadUInt32(true);

                    m_EntryTable.Add(m_Entry);
                }

                UInt32 dwTotalLengths = ((UInt32)m_Header.dwHeaderSize - (UInt32)TPsarStream.Position) / 2;
                Int32[] m_LengthsTable = new Int32[dwTotalLengths];

                for (Int32 i = 0; i < dwTotalLengths; i++)
                {
                    m_LengthsTable[i] = TPsarStream.ReadUInt16(true);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = PsarHashList.iGetNameFromHashList(m_Entry.m_NameHash).Replace("/", @"\");
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    var lpBuffer = PsarcChunks.iReadData(TPsarStream, m_Header, m_Entry, m_LengthsTable);
                    File.WriteAllBytes(m_FullPath, lpBuffer);

                    if (m_FileName == "NamesLookUp.txt")
                    {
                        PsarHashList.iLoadNames(lpBuffer);
                    }
                }

                TPsarStream.Dispose();
            }
        }
    }
}
