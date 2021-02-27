using System;

namespace Chess
{
    class GameInterface
    {
        public const int COUNT_OF_CELLS = 8;
        const int CELL_SIZE = 5;

        private void PrintLocalCup()
        {
            for(int i = 0; i < CELL_SIZE; ++i)
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
            for(int wallI = 0; wallI < COUNT_OF_CELLS; ++wallI)
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
            for(int i = 0; i < CELL_SIZE / 2 + 1; ++i)
            {
                if(i == (CELL_SIZE / 2 + 1) / 2)
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
            for(int i = 0; i < COUNT_OF_CELLS; ++i)
            {
                PrintCup();
                PrintWall(fealdInf, COUNT_OF_CELLS - i - 1);
            }
            PrintCup();

            for(int i = 0; i < COUNT_OF_CELLS; ++i)
            {
                Console.Write(" ");
                for(int localI = 0; localI < CELL_SIZE; ++localI)
                {
                    if(localI == CELL_SIZE / 2 + 1)
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

    abstract class Figure
    {
        public Figure(bool isPlayer, ConsoleColor color = ConsoleColor.White)
        {
            this.isPlayer = isPlayer;
            this.color = color;
            this.baseColor = color;
        }

        public enum Figures
        {
            Empty,
            King,
            Pawn,
            Queen,
            Bishop,
            Knight,
            Castle
        }
        public bool isPlayer = false;
        public ConsoleColor color;
        public ConsoleColor baseColor;

        public void ChangePlayer()
        {
            bool v = !isPlayer;
            isPlayer = v;
        }
        public void ResetColor()
        {
            color = baseColor;
        }

        public abstract Figures FigureIs();
        public abstract char FigureSymbol();
        public abstract bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing);
    }

    class King: Figure
    {
        public King(bool isPlayer, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        public override Figures FigureIs()
        {
            return Figures.King;
        }
        public override char FigureSymbol()
        {
            return 'K';
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            if(Math.Abs(numberFrom - numberTo) > 1 || Math.Abs(symbolFrom - symbolTo) > 1)
            {
                return false;
            }
            
            for(int numberInd = numberTo - 1; numberInd <= numberTo + 1; ++numberInd)
            {
                for (int symbolInd = symbolTo - 1; symbolInd <= symbolTo + 1; ++symbolInd)
                {
                    if (numberInd >= 0 && numberInd < 8 && symbolInd >= 0 && symbolInd < 8)
                    {
                        if(fealdInf.figure[numberInd, symbolInd].FigureIs() == Figures.King && !fealdInf.figure[numberInd, symbolInd].isPlayer)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
    class Pawn: Figure
    {
        public Pawn(bool isPlayer, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        public override Figures FigureIs()
        {
            return Figures.Pawn;
        }
        public override char FigureSymbol()
        {
            return '*';
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            int distance = 1;
            if(color != ConsoleColor.White)
            {
                distance = -1;
            }

            if (numberTo - numberFrom == distance && symbolTo == symbolFrom) // если пешка шагает вперед 
            {
                return fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.Empty;
            }
            if (numberTo - numberFrom == distance && Math.Abs(symbolTo - symbolFrom) == 1) // бъет по диагонали 
            {
                // там должен быть НЕ союзник, НЕ король, НЕ пустота 
                if (fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.King)
                {
                    return isInfForKing && !fealdInf.figure[numberFrom, symbolFrom].isPlayer;
                }
                if (fealdInf.figure[numberTo, symbolTo].isPlayer)
                {
                    return false;
                }
                if (fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.Empty)
                {
                    return false;
                }
                return true;
            }

            return false; // пешка может ТОЛЬКО шагать вперед ИЛИ бить по диагонали вперед (относительно пешки) 
        }
    }
    class Queen : Figure
    {
        public Queen(bool isPlayer, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        public override Figures FigureIs()
        {
            return Figures.Queen;
        }
        public override char FigureSymbol()
        {
            return 'Q';
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            bool isMoveLikeQ = false;
            int move = 1;

            if (numberFrom == numberTo) // двигается по горизонтали  
            {
                isMoveLikeQ = true;

                if (symbolTo - symbolFrom < 0)
                {
                    move = -1;
                }

                for (int i = symbolFrom + move; i != symbolTo; i+=move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[numberFrom, i].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }
            if(symbolFrom == symbolTo) // двигается по по вертикали 
            {
                isMoveLikeQ = true;

                if (numberTo - numberFrom < 0)
                {
                    move = -1;
                }

                for (int i = numberFrom + move; i != numberTo; i += move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[i, symbolFrom].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }
            if(numberTo - numberFrom == symbolTo - symbolFrom) // двигается по диагоали в 1 и 3 четвертях 
            {
                isMoveLikeQ = true;

                if (symbolTo - symbolFrom < 0 && numberTo - numberFrom < 0)
                {
                    move = -1;
                }

                for (int numberInd = numberFrom + move, symbolInd = symbolFrom + move; numberInd != numberTo; numberInd += move, symbolInd += move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[numberInd, symbolInd].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }
            else if(Math.Abs(numberTo - numberFrom) == Math.Abs(symbolTo - symbolFrom)) // двигается по любой диагонали 
            {
                isMoveLikeQ = true;

                if (numberTo - numberFrom < 0 && symbolTo - symbolFrom > 0)
                {
                    move = -1;
                }

                for (int numberInd = numberFrom + move, symbolInd = symbolFrom - move; numberInd != numberTo; numberInd += move, symbolInd -= move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[numberInd, symbolInd].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }

            if(!isMoveLikeQ)
            {
                return false;
            }

            // может ли занять клетку, там должны быть НЕ союзник И НЕ король
            if (fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.King)
            {
                return isInfForKing && !fealdInf.figure[numberFrom, symbolFrom].isPlayer;
            }
            if (fealdInf.figure[numberTo, symbolTo].isPlayer)
            {
                return false;
            }
            return true;
        }
    }
    class Bishop : Figure
    {
        public Bishop(bool isPlayer, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        public override Figures FigureIs()
        {
            return Figures.Bishop;
        }
        public override char FigureSymbol()
        {
            return 'B';
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            bool isMoveLikeB = false;
            int move = 1;

            
            if (numberTo - numberFrom == symbolTo - symbolFrom) // двигается по диагоали в 1 и 3 четвертях 
            {
                isMoveLikeB = true;

                if (symbolTo - symbolFrom < 0 && numberTo - numberFrom < 0)
                {
                    move = -1;
                }

                for (int numberInd = numberFrom + move, symbolInd = symbolFrom + move; numberInd != numberTo; numberInd += move, symbolInd += move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[numberInd, symbolInd].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }
            else if (Math.Abs(numberTo - numberFrom) == Math.Abs(symbolTo - symbolFrom)) // двигается по любой диагонали 
            {
                isMoveLikeB = true;

                if (numberTo - numberFrom < 0 && symbolTo - symbolFrom > 0)
                {
                    move = -1;
                }

                for (int numberInd = numberFrom + move, symbolInd = symbolFrom - move; numberInd != numberTo; numberInd += move, symbolInd -= move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[numberInd, symbolInd].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }

            if (!isMoveLikeB)
            {
                return false;
            }

            if (fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.King)
            {
                return isInfForKing && !fealdInf.figure[numberFrom, symbolFrom].isPlayer;
            }
            if (fealdInf.figure[numberTo, symbolTo].isPlayer)
            {
                return false;
            }
            // может ли занять клетку, там должны быть НЕ союзник И НЕ король
            return true;
        }
    }
    class Castle : Figure
    {
        public Castle(bool isPlayer, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        //public Figures FigureType { get; } = Figures.Castle; !!! 

        public override Figures FigureIs()
        {
            return Figures.Castle;
        }
        public override char FigureSymbol()
        {
            return 'C';
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            bool isMoveLikeC = false;
            int move = 1;

            if (numberFrom == numberTo) // двигается по горизонтали  
            {
                isMoveLikeC = true;

                if (symbolTo - symbolFrom < 0)
                {
                    move = -1;
                }

                for (int i = symbolFrom + move; i != symbolTo; i += move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[numberFrom, i].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }
            if (symbolFrom == symbolTo) // двигается по по вертикали 
            {
                isMoveLikeC = true;

                if (numberTo - numberFrom < 0)
                {
                    move = -1;
                }

                for (int i = numberFrom + move; i != numberTo; i += move) // может ли дойти (свободен ли путь) 
                {
                    if (fealdInf.figure[i, symbolFrom].FigureIs() != Figures.Empty)
                    {
                        return false;
                    }
                }
            }

            if (!isMoveLikeC)
            {
                return false;
            }

            // может ли занять клетку, там должны быть НЕ союзник И НЕ король
            if (fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.King)
            {
                return isInfForKing && !fealdInf.figure[numberFrom, symbolFrom].isPlayer;
            }
            if (fealdInf.figure[numberTo, symbolTo].isPlayer)
            {
                return false;
            }
            return true;
        }
    }
    class Knight : Figure
    {
        public Knight(bool isPlayer, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        public override Figures FigureIs()
        {
            return Figures.Knight;
        }
        public override char FigureSymbol()
        {
            return 'L';
        }

        private bool CanKnightAtack(FealdInf fealdInf, int numberTo, int symbolTo, bool isInfForKing, int numberFrom, int symbolFrom)
        {
            // может ли занять клетку, там должны быть НЕ союзник И НЕ король
            if (fealdInf.figure[numberTo, symbolTo].FigureIs() == Figures.King)
            {
                return isInfForKing && !fealdInf.figure[numberFrom, symbolFrom].isPlayer;
            }
            if (fealdInf.figure[numberTo, symbolTo].isPlayer)
            {
                return false;
            }
            return true;
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            if(Math.Abs(numberTo - numberFrom) == 2 && Math.Abs(symbolTo - symbolFrom) == 1)
            {
                return CanKnightAtack(fealdInf, numberTo, symbolTo, isInfForKing, numberFrom, symbolFrom);
            }
            if (Math.Abs(symbolTo - symbolFrom) == 2 && Math.Abs(numberTo - numberFrom) == 1)
            {
                return CanKnightAtack(fealdInf, numberTo, symbolTo, isInfForKing, numberFrom, symbolFrom);
            }
            return false;
        }
    }
    class Empty: Figure
    {
        public Empty(bool isPlayer = false, ConsoleColor color = ConsoleColor.White)
            : base(isPlayer, color)
        {
        }

        public override Figures FigureIs()
        {
            return Figures.Empty;
        }
        public override char FigureSymbol()
        {
            return ' ';
        }
        public override bool CanAtack(FealdInf fealdInf, int numberFrom, int symbolFrom, int numberTo, int symbolTo, bool isInfForKing)
        {
            return false;
        }
    }


    class FealdInf
    {
        public FealdInf()
        {
            for (int number = 0; number < GameInterface.COUNT_OF_CELLS; ++number)
            {
                for (int symbol = 0; symbol < GameInterface.COUNT_OF_CELLS; ++symbol)
                {
                    figure[number, symbol] = new Empty();
                }
            }

            // player army 
            figure[0, 4] = new King(true);
            figure[0, 3] = new Queen(true);
            figure[0, 2] = new Bishop(true);
            figure[0, 5] = new Bishop(true);
            figure[0, 1] = new Knight(true);
            figure[0, 6] = new Knight(true);
            figure[0, 0] = new Castle(true);
            figure[0, 7] = new Castle(true);
            for(int i = 0; i < 8; ++i)
            {
                figure[1, i] = new Pawn(true);
            }

            // PC army 
            figure[7, 4] = new King(false, ConsoleColor.DarkBlue);
            figure[7, 3] = new Queen(false, ConsoleColor.DarkBlue);
            figure[7, 2] = new Bishop(false, ConsoleColor.DarkBlue);
            figure[7, 5] = new Bishop(false, ConsoleColor.DarkBlue);
            figure[7, 1] = new Knight(false, ConsoleColor.DarkBlue);
            figure[7, 6] = new Knight(false, ConsoleColor.DarkBlue);
            figure[7, 0] = new Castle(false, ConsoleColor.DarkBlue);
            figure[7, 7] = new Castle(false, ConsoleColor.DarkBlue);
            for (int i = 0; i < 8; ++i)
            {
                figure[6, i] = new Pawn(false, ConsoleColor.DarkBlue);
            }
        }

        public void ChangePlayer()
        {
            for (int number = 0; number < GameInterface.COUNT_OF_CELLS; ++number)
            {
                for (int symbol = 0; symbol < GameInterface.COUNT_OF_CELLS; ++symbol)
                {
                    if(figure[number, symbol].FigureIs() != Figure.Figures.Empty)
                    {
                        figure[number, symbol].ChangePlayer();
                    }
                }
            }
        }

        public void ResetColor()
        {
            for (int number = 0; number < GameInterface.COUNT_OF_CELLS; ++number)
            {
                for (int symbol = 0; symbol < GameInterface.COUNT_OF_CELLS; ++symbol)
                {
                    figure[number, symbol].ResetColor();
                }
            }
        }

        public Figure[,] figure = new Figure[GameInterface.COUNT_OF_CELLS, GameInterface.COUNT_OF_CELLS];
    }

    static class Game
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

    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            FealdInf fealdInf = new FealdInf();
            GameInterface feald = new GameInterface();

            feald.PrintFeald(fealdInf);

            Game.Play(feald, fealdInf);
        }
    }
}
