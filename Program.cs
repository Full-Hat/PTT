using System;
using System.Runtime.InteropServices;
using System.Text;

namespace lab4_task2
{
    unsafe class Program
    {
        [DllImport("lab4-task2DLL.dll", EntryPoint = "checkPass", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool checkPass(StringBuilder message);
        [DllImport("lab4-task2DLL.dll", EntryPoint = "botDefense", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern bool botDefense(StringBuilder message);

        static void Main(string[] args)
        {
            StringBuilder str = new StringBuilder();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            str.Append(password);

            while(!checkPass(str))
            {
                Console.WriteLine("Wrong password, try again..");
                str.Clear();
                str.Append(Console.ReadLine());
            }

            Console.Write("Bot defense pass: ");
            password = Console.ReadLine();
            str.Clear();
            str.Append(password);


            while (!botDefense(str))
            {
                Console.WriteLine("Wrong password, try again..");
                str.Clear();
                str.Append(Console.ReadLine());
            }

            Console.WriteLine("Finally, correct password! :D");
        }
    }
}
