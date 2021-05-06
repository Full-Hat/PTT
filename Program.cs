using System;

using Rational;

namespace BigDouble
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{new string('~', 15)} Test {new string('~', 15)}"); 

            // test string format 
            RationalNumber r1 = new RationalNumber(1, 2);
            Console.WriteLine($"Normal: {r1}, with brackets: {r1.ToString(RationalNumber.FormatType.BRACKETS)}");
            RationalNumber r1Negative = new RationalNumber(2, 4, true);
            Console.WriteLine($"Normal: {r1Negative}, with brackets: {r1Negative.ToString(RationalNumber.FormatType.BRACKETS)}");

            Console.WriteLine();

            // test parsing
            RationalNumber r1Parsed = RationalNumber.Parse("1/3");
            Console.WriteLine($"1/3 is equal to {r1Parsed}");
            RationalNumber r1NegativeParsed = RationalNumber.Parse("(-1/3)");
            Console.WriteLine($"1/3 is equal to {r1NegativeParsed}");

            Console.WriteLine();

            // test normalizing
            RationalNumber n = new RationalNumber(2, 4);
            n.Normalize();
            Console.WriteLine($"Normalized (2, 4): {n}");

            Console.WriteLine();

            // test operations
            RationalNumber n1 = new RationalNumber(1, 2);
            RationalNumber n2 = new RationalNumber(3, 7);
            Console.WriteLine($"{n1} + {n2} = {n1 + n2}");
            Console.WriteLine($"{n1} - {n2} = {n1 - n2}");
            Console.WriteLine($"{n1} * {n2} = {n1 * n2}");
            Console.WriteLine($"{n1} / {n2} = {n1 / n2}");

            Console.WriteLine();

            // test comparators
            Console.WriteLine(n1 + (n1 == n2 ? " == " : " != ") + n2);
            Console.WriteLine(r1Parsed + (r1Parsed != n1 ? " != " : " == ") + n1);
            Console.WriteLine(n1 + (n1 == new RationalNumber(2, 4) ? " == " : " != ") + "2/4");

            Console.WriteLine();

            Console.WriteLine(r1Negative + (r1Negative > r1 ? " > " : " < ") + r1);
            Console.WriteLine(n1 + (n1 < n2 ? " < " : " > ") + n2);

            Console.WriteLine();

            Console.WriteLine($"Double: {(double)r1}");
            Console.WriteLine($"Float: {(float)n2}");
            RationalNumber rational = new RationalNumber(15, 9);
            Console.WriteLine($"Int: {(int)rational}");
        }
    }
}
