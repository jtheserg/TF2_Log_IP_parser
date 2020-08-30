using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace TF2_Log_IP_parser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const bool KeepPort = false;
            const string IP_pattern = @"\([0-9]*\.[0-9]*\.[0-9]*\.[0-9]*\:[0-9]*\)";

            if (args is null)
            {
                throw new ArgumentException(nameof(args));
            }

            // Only for debug
            int fileLoc = 0; // 0 = fail
            IEnumerable Input = null;

            if (File.Exists("RequiredFilesForExport\\Example.txt"))
            {
                fileLoc = 1; // When debuging
            }
            else if (File.Exists("Example.txt"))
            {
                fileLoc = 2; // When downloaded and run
            }
            else if (File.Exists("..\\..\\..\\RequiredFilesForExport\\Example.txt"))
            {
                fileLoc = 3; // When coding or something
            }

            switch (fileLoc)
            {
                case 1:
                    Input = File.ReadLines("RequiredFilesForExport\\Example.txt");
                    break;
                case 2:
                    Input = File.ReadLines("Example.txt");
                    break;
                case 3:
                    Input = File.ReadLines("..\\..\\..\\RequiredFilesForExport\\Example.txt");
                    break;
                default:
                    break;
            }

            if (fileLoc != 0)
            {
                // Actual code
                foreach (string Line in Input)
                {
                    string pLine = ParseFile(Line, IP_pattern, Garbage_Pattern(KeepPort));
                    if (pLine != string.Empty)
                    {
                        Console.WriteLine(pLine);
                    }
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
        private static string ParseFile(string Line, string IP_pattern, string Garbage_pattern)
        {
            return Regex.IsMatch(Line, IP_pattern)
                ? Regex.Replace(Regex.Match(Line, IP_pattern).Value, Garbage_pattern, string.Empty)
                : string.Empty;
        }
        private static string Garbage_Pattern(bool KeepPort)
        {
            return $@"\(|\){(KeepPort ? string.Empty : @"|\:[0-9]*")}";
        }
    }
}
