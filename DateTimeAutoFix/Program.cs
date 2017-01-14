using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DateTimeAutoFix
{
    class Program
    {
        static void Main(string[] args)
        {
            string strFileNameFormat = "yyyyMMdd_HHmmss";
            string strDirectory = "D:\\test\\";
            string fileName = null;

            string[] files = Directory.GetFiles(strDirectory);
            for (int i = 0, N = files.Count(); i < N; i++)
            {
                fileName = Path.GetFileNameWithoutExtension(files[i]);
                DateTime fileModificationDateTime;

                // Case 1: The file name is only date stamp as specified in format string
                bool success = DateTime.TryParseExact(fileName, strFileNameFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fileModificationDateTime);

                if(!success)
                {
                    // Case 2: A valid file name ends with suffix. Workaround: remove suffix
                    fileName = fileName.Substring(0, strFileNameFormat.Length);
                    success = DateTime.TryParseExact(fileName, strFileNameFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fileModificationDateTime);
                }

                if (success)
                {
                    Console.WriteLine("{0} \t\t\t {1}", fileName, fileModificationDateTime);
                }
                else
                {
                    Console.WriteLine("{0} - File format invalid", fileName);
                }

                File.SetCreationTime(files[i], fileModificationDateTime);
                File.SetLastWriteTime(files[i], fileModificationDateTime);
            }
            Console.ReadKey();
        }
    }
}
