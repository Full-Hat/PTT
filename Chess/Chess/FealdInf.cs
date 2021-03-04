using System;
using Chess.Figures;

namespace Chess
{
    public class FealdInf
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
            for (int i = 0; i < 8; ++i)
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
                    if (figure[number, symbol].FigureIs() != Figure.Figures.Empty)
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
}
