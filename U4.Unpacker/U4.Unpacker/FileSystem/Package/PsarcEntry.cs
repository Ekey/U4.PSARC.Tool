using System;

namespace U4.Unpacker
{
    class PsarcEntry
    {
        public String m_NameHash { get; set; }
        public Int32 dwEntryIndex { get; set; }
        public Int64 dwDecompressedSize { get; set; }
        public Int64 dwOffset { get; set; }
    }
}
