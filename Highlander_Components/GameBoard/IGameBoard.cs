using Highlander_Console.lander;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander_Console
{
    public interface IGameBoard
    {
        int Rows { get; set; }
        int Columns { get; set; }
        int[,] Board { get; }

        void InitializeBoard();

        void PrintBoard();

        void resetBoard();

        bool IsPositionValid(int row, int col);

        void AddHighlander(Highlander highlander);
    }
}
