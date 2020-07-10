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

            if (args! is null)
            {
                throw new ArgumentException(nameof(args));
            }

            IEnumerable Input = File.ReadLines("Example.txt");

            if (KeepPort)
            {
                foreach (string Line in Input)
                {
                    Console.Write(ParseFile(Line, IP_pattern, Garbage_Keep_Port));
                }
            }
            else
            {
                foreach (string Line in Input)
                {
                    Console.Write(ParseFile(Line, IP_pattern, Garbage_Delete_Port));
                }
            }
        }
        private static string ParseFile(string Line, string IP_pattern, string Garbage_pattern)
        {
            if (Regex.IsMatch(Line, IP_pattern))
            {
                return Regex.Replace(Regex.Match(Line, IP_pattern).Value, Garbage_pattern, "") + "\n";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
