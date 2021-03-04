using System;

namespace Chess.Figures
{
    public class Queen : Figure
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
            if (numberTo - numberFrom == symbolTo - symbolFrom) // двигается по диагоали в 1 и 3 четвертях 
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
            else if (Math.Abs(numberTo - numberFrom) == Math.Abs(symbolTo - symbolFrom)) // двигается по любой диагонали 
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

            if (!isMoveLikeQ)
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
}
