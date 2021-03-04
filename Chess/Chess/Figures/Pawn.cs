using System;

namespace Chess.Figures
{
    public class Pawn : Figure
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
            if (color != ConsoleColor.White)
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
}
