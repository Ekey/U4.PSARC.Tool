using System;

namespace U4.Unpacker
{
    class PsarcHeader
    {
        public UInt32 dwMagic { get; set; } // 0x52415350 (PSAR)
        public Int16 wMajorVersion { get; set; } // 1
        public Int16 wMinorVersion { get; set; } // 4
        public UInt32 dwCompressionType { get; set; } // 0x6C646F6F (oodl)
        public Int32 dwHeaderSize { get; set; }
        public Int32 dwEntrySize { get; set; }
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwChunkSize { get; set; } // 65536
        public Int32 dwUnknown { get; set; } // always 0
    }
}
