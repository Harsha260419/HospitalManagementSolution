using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Infrastructure.Logging
{
    public class Logger
    {
        private static string filePath = "errorlog.txt";

        public static void Log(Exception ex)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-------------------------------------------------");
                writer.WriteLine($"Date: {DateTime.Now}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine("-------------------------------------------------");
            }
        }
    }
}
