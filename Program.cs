using System;
using System.Runtime.InteropServices;
using System.Text;

namespace lab4_task2
{
    unsafe class Program
    {
        [DllImport("lab4-task2DLL.dll", EntryPoint = "getL", CharSet = CharSet.Ansi)]
        public static extern bool getL(StringBuilder message);

        static void Main(string[] args)
        {
            StringBuilder str = new StringBuilder();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            str.Append(password);

            while(!getL(str))
            {
                Console.WriteLine("Wrong password, try again..");
                str.Clear();
                str.Append(Console.ReadLine());
            }

            Console.WriteLine("You are finally enter");
        }
    }
}
