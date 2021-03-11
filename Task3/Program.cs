using System;
using System.Globalization;

namespace Task3
{
    class Program
    {
        private static void PrintAllLanguages()
        {
            Console.WriteLine("Program support:\n");

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            for(int i = 1; i < cultures.Length; ++i)
            {

                Console.Write("#" + i + ": " + cultures[i] + "-");
                Console.WriteLine(cultures[i]);
            }
        }
        public static void printMonths(CultureInfo culture)
        {
            for (int i = 1; i <= 12; i++)
            {
                DateTime now = new DateTime(2010, i, 1);
                Console.WriteLine(now.ToString("MMMM", culture));
                Console.WriteLine();
            }
        }
        
        public static bool setLanguage(out CultureInfo culture, string languageCode)
        {
            if(languageCode.Length == 0)
            {
                culture = null;
                return false;
            }

            try
            {
                culture =  new CultureInfo(languageCode);
            }
            catch
            {
                culture = null;
                return false;
            }

            return true;
        }

        public static int Main(string[] args)
        {
            PrintAllLanguages();

            Console.Write("\nEnter language code: ");
            string languageCode = Console.ReadLine();
            CultureInfo culture = new CultureInfo("");

            while (!setLanguage(out culture, languageCode))
            {
                Console.Write("There is no so lang. code, try another one: ");
                languageCode = Console.ReadLine();
            }

            Console.WriteLine("\n");

            printMonths(culture);

            return 0;
        }
    }
}