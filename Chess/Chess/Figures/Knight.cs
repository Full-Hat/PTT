using System;

namespace Chess.Figures
{
    public class Knight : Figure
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
            if (Math.Abs(numberTo - numberFrom) == 2 && Math.Abs(symbolTo - symbolFrom) == 1)
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
}
