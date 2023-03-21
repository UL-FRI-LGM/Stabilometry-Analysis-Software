using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = args[0].Split(',');

            Process[] process = Process.GetProcessesByName(input[1]);
            while (process.Length > 0)
            {
                System.Threading.Thread.Sleep(200);
                Console.WriteLine(process.Length);
                process = Process.GetProcessesByName(input[1]);
            }

            DeleteAllFiles(input[0]);
        }

        private static void DeleteAllFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            foreach (DirectoryInfo dir in directory.GetDirectories())
                dir.Delete(true);

            foreach (FileInfo file in directory.GetFiles())
                file.Delete();

        }
    }
}
