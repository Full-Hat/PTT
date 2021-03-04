using System;
using Chess.Figures;

namespace Chess
{
    public static class Game
    {
        private static bool IsCheck(FealdInf fealdInf, int nFrom, int sFrom)
        {
            for (int number = 0; number < GameInterface.COUNT_OF_CELLS; ++number)
            {
                for (int symbol = 0; symbol < GameInterface.COUNT_OF_CELLS; ++symbol)
                {
                    if (CanPlayerAtack(fealdInf, number, symbol, nFrom, sFrom, true))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private static void GetKingCoord(FealdInf fealdInf, out int number, out int symbol)
        {
            symbol = 0;
            for (number = 0; number < GameInterface.COUNT_OF_CELLS; ++number)
            {
                for (symbol = 0; symbol < GameInterface.COUNT_OF_CELLS; ++symbol)
                {
                    if (fealdInf.figure[number, symbol].isPlayer)
                    {
                        if (fealdInf.figure[number, symbol].FigureIs() == Figure.Figures.King)
                        {
                            return;
                        }
                    }
                }
            }
        }
        private static bool IsCheck(FealdInf fealdInf)
        {
            int number, symbol;
            GetKingCoord(fealdInf, out number, out symbol);

            return IsCheck(fealdInf, number, symbol);
        }

        private static bool CanPlayerSchooseFigure(FealdInf fealdInf, int number, int intSymbol)
        {
            return fealdInf.figure[number, intSymbol].FigureIs() != Figure.Figures.Empty &&
                fealdInf.figure[number, intSymbol].isPlayer &&
                CanFigureMove(fealdInf, number, intSymbol);
        }

        private static bool CanFigureMove(FealdInf fealdInf, int numberFrom, int symbolFrom)
        {
            for (int numberTo = 0; numberTo < GameInterface.COUNT_OF_CELLS; ++numberTo)
            {
                for (int symbolTo = 0; symbolTo < GameInterface.COUNT_OF_CELLS; ++symbolTo)
                {
                    if (CanPlayerAtack(fealdInf, numberFrom, symbolFrom, numberTo, symbolTo, false))
                    {
                        Figure coordToTemp = fealdInf.figure[numberTo, symbolTo];
                        fealdInf.figure[numberTo, symbolTo] = fealdInf.figure[numberFrom, symbolFrom];
                        fealdInf.figure[numberFrom, symbolFrom] = new Empty();

                        bool isCheck = IsCheck(fealdInf);

                        fealdInf.figure[numberFrom, symbolFrom] = fealdInf.figure[numberTo, symbolTo];
                        fealdInf.figure[numberTo, symbolTo] = coordToTemp;

                        if (!isCheck)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private static bool IsCheckmate(FealdInf fealdInf)
        {
            for (int number = 0; number < GameInterface.COUNT_OF_CELLS; ++number)
            {
                for (int symbol = 0; symbol < GameInterface.COUNT_OF_CELLS; ++symbol)
                {
                    if (fealdInf.figure[number, symbol].isPlayer && fealdInf.figure[number, symbol].FigureIs() != Figure.Figures.Empty)
                    {
                        if (CanFigureMove(fealdInf, number, symbol))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool CanPlayerAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing = false)
        {
            if (numberFrom == numberTo && symbolFrom == symbolTo)
            {
                return false;
            }
            if (fealdInf.figure[numberTo, symbolTo].isPlayer)
            {
                return isInfForKing &&
                    fealdInf.figure[numberFrom, symbolFrom].CanAtack(fealdInf, numberFrom, symbolFrom, numberTo, symbolTo, isInfForKing);
            }
            if (!fealdInf.figure[numberFrom, symbolFrom].CanAtack(fealdInf, numberFrom, symbolFrom, numberTo, symbolTo, isInfForKing))
            {
                return false;
            }

            return true;
        }

        private static void GetFigureCoord(FealdInf fealdInf, out int numberCoordFrom, out int symbolCoordFrom)
        {
            while (true)
            {
                char symbol;
                Console.Write("Enter symbol [A; H]: ");
                while (!char.TryParse(Console.ReadLine(), out symbol) || symbol < 'A' || symbol > 'H')
                {
                    Console.Write("Wrong input. Symbol must be [A; H], try again: ");
                }

                int number;
                Console.Write("Enter number [1; 8]: ");
                while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > 8)
                {
                    Console.Write("Wrong input. Number must be [1; 8], try again: ");
                }

                // convert to index form ([0; 8]) 
                symbolCoordFrom = symbol - 65;
                numberCoordFrom = number - 1;

                if (CanPlayerSchooseFigure(fealdInf, numberCoordFrom, symbolCoordFrom))
                {
                    Console.WriteLine("You had choosen figure! So, choose coordinates to attack: ");
                    break;
                }
                else
                {
                    Console.WriteLine("You can't choose this coordinates! Try again..");
                    continue;
                }
            }

            //Console.WriteLine("Coordinates: " + numberCoordFrom + " : " + symbolCoordFrom); // test inf 
        }

        private static void GetAtackCoord(FealdInf fealdInf, out int numberCoordTo, out int symbolCoordTo, int numberCoordFrom, int symbolCoordFrom)
        {
            while (true)
            {
                char symbol;
                Console.Write("Enter symbol [A; H]: ");
                while (!char.TryParse(Console.ReadLine(), out symbol) || symbol < 'A' || symbol > 'H')
                {
                    Console.Write("Wrong input. Symbol must be [A; H], try again: ");
                }

                int number;
                Console.Write("Enter number [1; 8]: ");
                while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > 8)
                {
                    Console.Write("Wrong input. Number must be [1; 8], try again: ");
                }

                // convert to index form ([0; 8]) 
                symbolCoordTo = symbol - 65;
                numberCoordTo = number - 1;

                if (CanPlayerAtack(fealdInf, numberCoordFrom, symbolCoordFrom, numberCoordTo, symbolCoordTo))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You can't choose this coordinates! Try again..");
                    continue;
                }
            }

            //Console.WriteLine("Coordinates: " + numberCoordTo + " : " + symbolCoordTo); // test inf 
        }

        private static bool PlayerRound(FealdInf fealdInf)
        {
            bool isCheck, isCheckmate;
            do
            {
                int numberCoordFrom, symbolCoordFrom;
                GetFigureCoord(fealdInf, out numberCoordFrom, out symbolCoordFrom);

                int numberCoordTo, symbolCoordTo;
                GetAtackCoord(fealdInf, out numberCoordTo, out symbolCoordTo, numberCoordFrom, symbolCoordFrom);


                Figure coordToTemp = fealdInf.figure[numberCoordTo, symbolCoordTo];
                fealdInf.figure[numberCoordTo, symbolCoordTo] = fealdInf.figure[numberCoordFrom, symbolCoordFrom];
                fealdInf.figure[numberCoordFrom, symbolCoordFrom] = new Empty();

                isCheck = IsCheck(fealdInf);
                fealdInf.ChangePlayer();
                isCheckmate = IsCheckmate(fealdInf);
                fealdInf.ChangePlayer();
                if (isCheck)
                {
                    Console.WriteLine("It's a check! Help you king! Try another figure or move..");
                    fealdInf.figure[numberCoordFrom, symbolCoordFrom] = fealdInf.figure[numberCoordTo, symbolCoordTo];
                    fealdInf.figure[numberCoordTo, symbolCoordTo] = coordToTemp;
                }
                if (isCheckmate)
                {
                    Console.WriteLine("Checkmate! One army fell, the other won..");
                    int number, symbol;
                    GetKingCoord(fealdInf, out number, out symbol);
                    Console.WriteLine($"Congratulations to {fealdInf.figure[number, symbol].baseColor.ToString()}!");
                    return true;
                }
            }
            while (isCheck);

            Console.WriteLine("You make an attack!");
            return false;
        }

        public static void Play(GameInterface feald, FealdInf fealdInf)
        {
            while (true)
            {
                if (PlayerRound(fealdInf))
                {
                    return;
                }
                feald.PrintFeald(fealdInf);
                fealdInf.ChangePlayer();

                fealdInf.ResetColor();

                int number, symbol;
                GetKingCoord(fealdInf, out number, out symbol);
                Console.WriteLine($"\n\n{fealdInf.figure[number, symbol].baseColor} turn\n");
            }
        }
    }
}
