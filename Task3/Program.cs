using System;

namespace Task2
{
    class Program
    {
        public static double StringToDecimal(string str)
        {
            int i = 0;
            bool isNegative = false;

            if(str.Length == 0)
            {
                throw new Exception("Empty string, can't convert");
            }
            if(str[i] == '-')
            {
                isNegative = true;
                ++i;
            }
            if (str[i] == '+')
            {
                ++i;
            }

            double result = 0.0;
            for (; str[i] != '.'; ++i)
            {
                result = result * 10.0 + (str[i] - '0');

                if (!char.IsDigit(str[i]) || i == str.Length - 1)
                {
                    throw new Exception("Invalid data to convertation");
                }

                if (i > 308)
                {
                    throw new Exception("Can't convert string, too big value");
                }
            }

            ++i;
            int afterPointCount = 1;
            for (; i < str.Length; ++i)
            {
                result += (str[i] - '0') / (Math.Pow(10.0, afterPointCount++));

                if (!char.IsDigit(str[i]))
                {
                    throw new Exception("Invalid data to convertation");
                }
            }

            result = isNegative ? -result : result;

            return result;
        }

        public static int Main(string[] args)
        {
            Console.Write("Enter something: ");
            string strNumber = Console.ReadLine();

            try
            {
                Console.WriteLine($"Double: {StringToDecimal(strNumber)}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
    }
}