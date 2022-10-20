using System;

namespace U4.Unpacker
{
    class PsarcEntry
    {
        public String m_NameHash { get; set; }
        public Int32 dwEntryIndex { get; set; }
        public Int32 bFlag1 { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 bFlag2 { get; set; }
        public UInt32 dwOffset { get; set; }
    }
}
