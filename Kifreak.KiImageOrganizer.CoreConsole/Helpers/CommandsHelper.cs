using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.CoreConsole.Helpers
{
    public static class CommandsHelper
    {
        public static void ForeachFiles(string directory, Func<string, Task> action)
        {
            var allFiles = System.IO.Directory.GetFiles(directory);
            foreach (string file in allFiles)
            {
                Task tsk = action.Invoke(file);
                tsk.Wait();
            }
        }
        public static List<string> GetFilesInaFolder(string carpeta)
        {
            List<string> archivos = new List<string>();

            try
            {
                // Obtiene todos los archivos en la carpeta actual
                archivos.AddRange(Directory.GetFiles(carpeta));

                // Obtiene todas las subcarpetas en la carpeta actual
                string[] subcarpetas = Directory.GetDirectories(carpeta);

                // Recorre las subcarpetas y obtiene los archivos de cada una de ellas (recursión)
                foreach (string subcarpeta in subcarpetas)
                {
                    archivos.AddRange(GetFilesInaFolder(subcarpeta));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder a la carpeta {carpeta}: {ex.Message}");
            }

            return archivos;
        }
    


        public static Dictionary<string, string> GetBasicTags()
        {
            return new Dictionary<string, string>
            {
                {"City", "City where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM"},
                {"Road", "Road where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM"},
                {"Village", "Village where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM"},
                {"Country", "Country where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM"},
                {"County", "County  where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM"},
                {"AmenityType", "If the photo was taken in a specific place (like a restaurant or museum) indicate the type."},
                {"AmenityName", "If the photo was taken in a specific place indicate the place's name (My Restaurant, British museum, etc)"},
                {"Date", "Date when the photo was taken (format: yyyy-MM-dd)"},
                {"DateTime", "Date and Time when the photo was taken (format: yyyy-MM-dd HH_mm_ss)"},
                {"Time", "Time when the photo was taken (format: HH_mm_ss)"},
                {"YearMonth", "Year and Month when the photo was taken (format: yyyy-MM)"},
                {"Noop", "Do nothing."},
                {"Alternatives", "This is not a keyword. You can use an alternative in case your option return an empty result: For example: Village|City or City|Noop" }
            };
        }
    }
}