using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            FealdInf fealdInf = new FealdInf();
            GameInterface feald = new GameInterface();

            feald.PrintFeald(fealdInf);

            Game.Play(feald, fealdInf);

            feald.PrintFeald(fealdInf);
        }
    }
}
