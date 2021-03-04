using System;

namespace Chess
{
    public class GameInterface
    {
        public const int COUNT_OF_CELLS = 8;
        const int CELL_SIZE = 5;

        private void PrintLocalCup()
        {
            for (int i = 0; i < CELL_SIZE; ++i)
            {
                Console.Write("#");
            }
        }
        private void PrintCup()
        {
            for (int i = 0; i < COUNT_OF_CELLS; ++i)
            {
                Console.Write("#");
                PrintLocalCup();
            }
            Console.Write("#\n");
        }

        private void PrintLocalWall(FealdInf fealdInf, int lineCount, bool printInf)
        {
            // тут идет перебор каждой ячейки в строке
            for (int wallI = 0; wallI < COUNT_OF_CELLS; ++wallI)
            {
                Console.Write("#");
                for (int celValueI = 0; celValueI < CELL_SIZE; ++celValueI)
                {
                    if (celValueI == CELL_SIZE / 2 + 1 && printInf)
                    {
                        Console.ForegroundColor = fealdInf.figure[lineCount, wallI].color;
                        // Тут должен знать о фигуре
                        Console.Write(fealdInf.figure[lineCount, wallI].FigureSymbol());

                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
            Console.Write("#");
        }
        private void PrintWall(FealdInf fealdInf, int lineCount)
        {
            for (int i = 0; i < CELL_SIZE / 2 + 1; ++i)
            {
                if (i == (CELL_SIZE / 2 + 1) / 2)
                {
                    // must type 
                    PrintLocalWall(fealdInf, lineCount, true);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"  {lineCount + 1}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    // mustn't type 
                    PrintLocalWall(fealdInf, lineCount, false);
                }
                Console.Write("\n");
            }
        }

        public void PrintFeald(FealdInf fealdInf)
        {
            for (int i = 0; i < COUNT_OF_CELLS; ++i)
            {
                PrintCup();
                PrintWall(fealdInf, COUNT_OF_CELLS - i - 1);
            }
            PrintCup();

            for (int i = 0; i < COUNT_OF_CELLS; ++i)
            {
                Console.Write(" ");
                for (int localI = 0; localI < CELL_SIZE; ++localI)
                {
                    if (localI == CELL_SIZE / 2 + 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write((char)(i + 1 + 64));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
            Console.Write("\n\n");
        }
    }
}
