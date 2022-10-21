using System;

namespace U4.Unpacker
{
    [Flags]
    public enum PsarcCompTypes : UInt32
    {
        ZLIB = 0x62696C7A,
        OODLE = 0x6C646F6F,
    }
}
