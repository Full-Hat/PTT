using System;

namespace Chess.Figures
{
    public class Castle : Figure
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
}
