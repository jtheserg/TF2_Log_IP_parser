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
            const string IP_pattern = @"\([0-9]*\.[0-9]*\.[0-9]*\.[0-9]*\:[0-9]*\)";
            const string Garbage_Keep_Port = @"\(|\)";
            const string Garbage_Delete_Port = @"\(|\)|\:[0-9]*";
            bool KeepPort = false;

            if (args is null)
            {
                throw new ArgumentException(nameof(args));
            }

            // Only for debug
            int fileLoc = 0;
            IEnumerable Input = null;
            if (File.Exists("..\\..\\..\\RequiredFilesForExport\\Example.txt"))
            {
                fileLoc = 1;
            }
            else if (File.Exists("Example.txt"))
            {
                fileLoc = 2;
            }

            switch (fileLoc)
            {
                case 1:
                    Input = File.ReadLines("..\\..\\..\\RequiredFilesForExport\\Example.txt");
                    break;
                case 2:
                    Input = File.ReadLines("Example.txt");
                    break;
                default:
                    break;
            }

            if (fileLoc != 0)
            {
                // Actual code
                foreach (string Line in Input)
                {
                    string pLine = ParseFile(Line, IP_pattern, KeepPort ? Garbage_Keep_Port : Garbage_Delete_Port);
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
            if (Regex.IsMatch(Line, IP_pattern))
            {
                return Regex.Replace(Regex.Match(Line, IP_pattern).Value, Garbage_pattern, string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
