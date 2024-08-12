using Highlander_Components.lander;
using System.Collections.Generic;

namespace Highlander_Component.GameBoard
{
    public interface IGameBoard
    {
        int Rows { get; set; }
        int Columns { get; set; }
        int[,] Board { get; }

        void InitializeBoard();

        void PrintBoard();

        bool IsPositionValid(int row, int col);

        void ClearBoard();

        void UpdateBoard(List<Highlander> highlanders);
    }
}
