// globalization 
// string to double 

using System;
using System.Globalization;

namespace Task2
{
    class Program
    {
        private static void DisplaySupportedLanguages()
        {
            Console.WriteLine("Supported languages:\n");

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (CultureInfo culture in cultures)
            {
                Console.Write(culture.Name + " - ");
                Console.WriteLine(culture.EnglishName);
            }
        }

        public static int Main(string[] args)
        {
            DisplaySupportedLanguages();

            Console.Write("\nEnter the language: ");
            string lang = Console.ReadLine();
            CultureInfo culture = new CultureInfo("");
            bool error = false;

            do
            {
                try
                {
                    culture = new CultureInfo(lang);
                    error = false;
                }
                catch
                {
                    Console.Write("Input error. Please enter the language again: ");
                    lang = Console.ReadLine();
                    error = true;
                }
            } while (error);

            Console.WriteLine("\n");

            for (int i = 1; i <= 12; i++)
            {
                DateTime now = new DateTime(2007, i, 1);
                Console.WriteLine(now.ToString("MMMM", culture));
                Console.WriteLine();
            }

            return 0;
        }
    }
}