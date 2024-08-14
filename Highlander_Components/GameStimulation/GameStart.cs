using Highlander_Component.GameBoard;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Components.GameStimulation
{
    public class GameStart
    {
        private static Random r = new Random();

        public static void PlaceHighlanders(IGameBoard<Highlander> gameBoard, List<Highlander> highlanders)
        {
            foreach (var highlander in highlanders)
            {
                bool placed = false;

                while (!placed)
                {
                    var row = r.Next(gameBoard.Rows);
                    var col = r.Next(gameBoard.Columns);

                    if (gameBoard.IsPositionValid(row, col))
                    {
                        // Update highlander position
                        highlander.Position = (row, col);

                        // Place highlander on the board
                        gameBoard.Board[row, col].Add(highlander);
                        placed = true;
                    }
                }
            }
        }

           
        
    }
}
