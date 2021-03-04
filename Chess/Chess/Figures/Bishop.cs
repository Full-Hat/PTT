using System;

namespace Chess.Figures
{
    public class Bishop : Figure
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
}
