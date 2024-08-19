using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Component.GameBoard
{
    public class GameBoard<T> : IGameBoard<T>
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public List<T>[,] Board { get; set; }

        public GameBoard(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Board = new List<T>[Rows, Columns];
            InitializeBoard();
        }


        public void InitializeBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Board[i, j] = new List<T>();
                }
            }
        }

        public bool IsPositionValid(int row, int col)
        {
            return row >= 0 && row < Rows && col >= 0 && col < Columns;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Board[i, j].Clear();
                }
            }
        }

        public void AddItem(T item, int row, int col)
        {
            Board[row, col].Add(item);
        }

        public void RemoveItem(T item, int row, int col)
        {
            Board[row, col].Remove(item);
        }

        // Print the game board
        public void PrintBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    // Print the list of highlanders at each position
                    Console.Write("[");
                    foreach (var item in Board[i, j])
                    {
                        int type = item is GoodHighlander ? 1 : 2;
                        Console.Write(type + " ");
                    }
                    Console.Write("] ");
                }
                // Move to the next row
                Console.WriteLine();
            }
        }

        public void InitializeBoard(int Rows, int Columns)
        {
            throw new NotImplementedException();
        }
    }
}
