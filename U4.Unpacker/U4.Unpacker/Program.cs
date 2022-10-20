using System;
using System.IO;

namespace U4.Unpacker
{
    class Program
    {
        private static String m_Title = "UNCHARTED: Legacy of Thieves Collection PSARC Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    U4.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of PSARC file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    U4.Unpacker E:\\Games\\U4\\Uncharted4_data\\data\\fonts.psarc D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_PsarcFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_PsarcFile))
            {
                Utils.iSetError("[ERROR]: Input PSARC file -> " + m_PsarcFile + " <- does not exist");
                return;
            }

            if (!File.Exists("oo2core_9_win64.dll"))
            {
                Utils.iSetError("[ERROR]: Unable to find oo2core_9_win64.dll module. Copy this library from game folder");
                return;
            }

            PsarcUnpack.iDoIt(m_PsarcFile, m_Output);
        }
    }
}
