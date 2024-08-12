using Highlander_Component.GameBoard;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Components.GameStimulation
{
    public class GameStart
    {
        private static Random r = new Random();

        public static void PlaceHighlanders(IGameBoard gameBoard, List<Highlander> highlanders)
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
                        // update highlander position
                        highlander.Position = (row, col);

                        // place highlander on the board
                        if (highlander is GoodHighlander)
                        {
                            gameBoard.Board[row, col] = 1;
                        }
                        else if (highlander is BadHighlander)
                        {
                            gameBoard.Board[row, col] = 2;
                        }
                        placed = true;
                    }
                }
            }
        }

           
        
    }
}
