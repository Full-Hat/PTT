using System;

namespace Chess.Figures
{
    public class King : Figure
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
            if (Math.Abs(numberFrom - numberTo) > 1 || Math.Abs(symbolFrom - symbolTo) > 1)
            {
                return false;
            }

            for (int numberInd = numberTo - 1; numberInd <= numberTo + 1; ++numberInd)
            {
                for (int symbolInd = symbolTo - 1; symbolInd <= symbolTo + 1; ++symbolInd)
                {
                    if (numberInd >= 0 && numberInd < 8 && symbolInd >= 0 && symbolInd < 8)
                    {
                        if (fealdInf.figure[numberInd, symbolInd].FigureIs() == Figures.King && !fealdInf.figure[numberInd, symbolInd].isPlayer)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
