using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander_Console
{
    public class GameBoard : IGameBoard
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int[,] Board { get; set; } // game board size
        public GameBoard(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Board = new int[Rows, Columns];
            InitializeBoard();
        }

        
        public void InitializeBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Board[i, j] = 0;
                }
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write(Board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void resetBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Board[i, j] = 0;
                }
            }
        }

        public bool IsPositionValid(int row, int col)
        {
            return row >= 0 && row < Rows && col >= 0 && col < Columns;
        }

        //TODO: Implement method to place highlanders on the board
        // 0 - empty cell
        // 1 - good highlander
        // 2 - bad highlander
        public void PlaceHighlander(int row, int col, int highlanderType)
        {
            if (IsPositionValid(row, col))
            {
                Board[row, col] = highlanderType;
            }
        }

        //TODO: Implement method to remove dead highlanders from the board

    }
}
