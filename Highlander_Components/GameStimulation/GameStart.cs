using Highlander_Component.GameBoard;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Components.GameStimulation
{
    public class GameStart
    {
        private static Random r = new Random();
        private static List<(int, int)> occupiedPositions = new List<(int, int)>();
        public static void PlaceHighlanders(IGameBoard<Highlander> gameBoard, List<Highlander> highlanders)
        {
            foreach (Highlander highlander in highlanders)
            {
                // check if the highlander is already placed or not
                bool placed = false;

                while (!placed)
                {
                    int row = r.Next(gameBoard.Rows);
                    int col = r.Next(gameBoard.Columns);

                    if (gameBoard.IsPositionValid(row, col) && !occupiedPositions.Contains((row, col)))
                    {
                        // Update highlander position
                        highlander.Position = (row, col);

                        // Place highlander on the board
                        gameBoard.Board[row, col].Add(highlander);

                        // Add position to occupied positions
                        occupiedPositions.Add((row, col));
                        placed = true;
                    }
                }
            }
        }

           
        
    }
}
