using System;

namespace Chess.Figures
{
    public abstract class Figure
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
}
