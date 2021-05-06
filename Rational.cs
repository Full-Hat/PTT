using System;  

namespace Rational
{
    class RationalNumber : IComparable<RationalNumber>, IComparable<double>
    {
        private uint Numerator { get; set; }
        private uint Denominator { get; set; }
        private bool IsNegative { get; set; }

        public enum FormatType
        {
            NORMAL,
            BRACKETS
        }

        public RationalNumber(uint numerator, uint denominator, bool isNegative = false)
        {
            Numerator = numerator;
            Denominator = denominator;
            IsNegative = isNegative;
        }
        public RationalNumber(string number) // format : a/b or (a/b) with any count of spaces   
        {
            RationalNumber r = Parse(number);
            Numerator = r.Numerator;
            Denominator = r.Denominator;
            IsNegative = r.IsNegative;
        }
        static public RationalNumber Parse(string data)
        {
            uint Numerator;
            uint Denominator;
            bool IsNegative = false;

            data = data.Trim();
            if (data.Length == 0)
                throw new Exception("Invalid format");

            if(data[0] == '(')
            {
                data = data.Remove(0, 1);
                data = data.Trim();
                if (data.Length == 0)
                    throw new Exception("Invalid format");
            }

            if (data[0] == '-' || data[0] == '+')
            {
                if (data[0] == '-')
                {
                    IsNegative = true;
                }

                data = data.Remove(0, 1);
                data = data.Trim();
                if (data.Length == 0)
                    throw new Exception("Invalid format");
            }

            string numerator = string.Empty;
            foreach (char ch in data)
            {
                if (!char.IsDigit(ch))
                {
                    break;
                }
                numerator += ch;
            }
            Numerator = uint.Parse(numerator);

            data = data.Remove(0, numerator.Length);
            data = data.Trim();
            if (data.Length == 0)
                throw new Exception("Invalid format");
            if (data[0] != '/')
                throw new Exception("Invalid format");
            data = data.Remove(0, 1);
            data = data.Trim();
            if (data.Length == 0)
                throw new Exception("Invalid format");

            string denominator = string.Empty;
            foreach (char ch in data)
            {
                if (!char.IsDigit(ch))
                {
                    break;
                }
                denominator += ch;
            }
            Denominator = uint.Parse(denominator);
            data = data.Remove(0, denominator.Length);
            data = data.Trim();

            if(data.Length != 0 && data[0] == ')')
            {
                data = data.Remove(0, 1);
                data = data.Trim();
            }
            if (data.Length != 0)
                throw new Exception("Invalid format");

            return new RationalNumber(Numerator, Denominator, IsNegative);
        } 

        // Math operations
        public static RationalNumber operator+ (RationalNumber r1, RationalNumber r2)
        {
            long newNum = (r1.IsNegative ? -1 : 1) * r1.Numerator * r2.Denominator + (r2.IsNegative ? -1 : 1) * r2.Numerator * r1.Denominator;
            uint newDen = r1.Denominator * r2.Denominator;
            RationalNumber temp = new RationalNumber((uint)newNum, newDen, newNum < 0);
            temp.Normalize();
            return temp;
        }
        public static RationalNumber operator- (RationalNumber r1, RationalNumber r2)
        {
            long newNum = (r1.IsNegative ? -1 : 1) * r1.Numerator * r2.Denominator - (r2.IsNegative ? -1 : 1) * r2.Numerator * r1.Denominator;
            uint newDen = r1.Denominator * r2.Denominator;
            RationalNumber temp = new RationalNumber((uint)newNum, newDen, newNum < 0);
            temp.Normalize();
            return temp;
        }
        public static RationalNumber operator* (RationalNumber r1, RationalNumber r2)
        {
            RationalNumber temp = new RationalNumber(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator, r1.IsNegative ^ r2.IsNegative);
            temp.Normalize();
            return temp;
        }
        public static RationalNumber operator/ (RationalNumber r1, RationalNumber r2)
        {
            RationalNumber temp = r1 * new RationalNumber(r2.Denominator, r2.Numerator, r2.IsNegative);
            temp.Normalize();
            return temp;
        }

        public static RationalNumber operator +(RationalNumber r1, uint num2)
        {
            long newNum = r1.Numerator + num2 * r1.Denominator;
            bool isNeg = false;
            if (newNum < 0)
            {
                isNeg = true;
                newNum = -newNum;
            }

            RationalNumber temp = new RationalNumber((uint)newNum, r1.Denominator, isNeg);
            temp.Normalize();
            return temp;
        }

        public static RationalNumber operator *(RationalNumber r1, uint num2)
        {
            RationalNumber temp = new RationalNumber(r1.Numerator * num2, r1.Denominator);
            temp.Normalize(); 
            return temp;
        }

        // Utils
        public void Normalize()
        {
            uint gcd = GCD(Numerator, Denominator);
            if (gcd == 0)
                return;
            Numerator /= gcd;
            Denominator /= gcd;
        }

        private static uint GCD(uint a, uint b)
        {
            if (a == 0)
            {
                return b;
            }
            else
            {
                while (b != 0)
                    if (a > b) a -= b; else b -= a;
                return a;
            }
        } 

        // comparators 
        public int CompareTo(RationalNumber other)
        {
            return this == other ? 1 : (this > other ? 1 : -1);
        }

        public int CompareTo(double other)
        {
            return this == other ? 1 : (this > other ? 1 : -1);
        }

        // convertors
        public static explicit operator double(RationalNumber r)
        {
            return (r.IsNegative ? -1 : 1) * (double)r.Numerator / r.Denominator;
        }

        public static explicit operator float(RationalNumber r)
        {
            return r.IsNegative ? -1 : 1 * (float)r.Numerator / r.Denominator;
        }

        public static explicit operator short(RationalNumber r)
        {
            return (short)(r.IsNegative ? -1 : 1 * (short)(r.Numerator / r.Denominator));
        }

        public static explicit operator int(RationalNumber r)
        {
            return r.IsNegative ? -1 : 1 * (int)(r.Numerator / r.Denominator);
        }

        public static explicit operator long(RationalNumber r)
        {
            return r.IsNegative ? -1 : 1 * (long)(r.Numerator / r.Denominator);
        }

        public static bool operator >(RationalNumber r1, RationalNumber r2)
        {
            r1.Normalize();
            r2.Normalize();
            return (double)r1 > (double)r2;
        }

        public static bool operator >(RationalNumber r1, double num)
        {
            r1.Normalize();
            return (double)r1 > num;
        }

        public static bool operator <(RationalNumber r1, RationalNumber r2)
        {
            r1.Normalize();
            r2.Normalize();
            return (double)r1 < (double)r2;
        }

        public static bool operator <(RationalNumber r1, double num)
        {
            r1.Normalize();
            return (double)r1 < num;
        }

        public static bool operator ==(RationalNumber r1, RationalNumber r2)
        {
            r1.Normalize();
            r2.Normalize();
            return (r1.Numerator == r2.Numerator) && (r1.Denominator == r2.Denominator) && (r1.IsNegative == r2.IsNegative);
        }

        public static bool operator ==(RationalNumber r1, double num)
        {
            r1.Normalize();
            return (Math.Abs((double)r1 - num) < 0.00001) && (Math.Abs((double)r1 - num) > -0.00001);
        }

        public static bool operator !=(RationalNumber r1, RationalNumber r2)
        {
            return !(r1 == r2);
        }

        public static bool operator !=(RationalNumber r1, double num)
        {
            return !(r1 == num);
        }

        public override string ToString()
        {
            return ToString(FormatType.NORMAL);
        }

        public string ToString(FormatType formatType = FormatType.NORMAL)
        {
            switch(formatType)
            {
                case FormatType.NORMAL:
                    return (IsNegative ? "-" : "") + Numerator.ToString() + "/" + Denominator.ToString();
                case FormatType.BRACKETS:
                    return "(" + (IsNegative ? "-" : "") + Numerator.ToString() + "/" + Denominator.ToString() + ")";
            }

            throw new Exception("Can't use this format type");
        }  
    }
}
