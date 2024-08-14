using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Component.GameBoard
{
    public interface IGameBoard<T>
    {
        int Rows { get; } // Number of rows in the game board
        int Columns { get; } // Number of columns in the game board
        List<T>[,] Board { get;} // Game board with highlanders at different positions

        // Initialize the game board
        void InitializeBoard(); 

        // Print the game board
        void PrintBoard(); 

        // Check if a position is valid
        bool IsPositionValid(int row, int col); 

        // Clear the game board
        void ClearBoard();

        // Add an item to the game board
        void AddItem(T item, int row, int col);

        // Remove an item from the game board
        void RemoveItem(T item, int row, int col);

        // Update the game board
        void UpdateBoard(List<T> items, Func<T, (int, int)> getPosition, Func<T, bool> isAlive);

    }
}
