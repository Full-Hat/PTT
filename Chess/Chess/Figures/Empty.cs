using System;

namespace Chess.Figures
{
    public class Empty : Figure
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
}
