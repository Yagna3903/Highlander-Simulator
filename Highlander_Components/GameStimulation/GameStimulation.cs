using Highlander_Component.GameBoard;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Highlander_Components.GameStimulation
{
    public class GameStimulation
    {
        // Declare the game board and highlanders
        private IGameBoard<Highlander> gameBoard;
        private List<Highlander> highlanders;


        public GameStimulation(IGameBoard<Highlander> gameBoard, List<Highlander> highlanders)
        {
            this.gameBoard = gameBoard;
            this.highlanders = highlanders;
            GameStart.PlaceHighlanders(this.gameBoard, this.highlanders);
        }


        public void RunSimulation(int maxIterations)
        {
            Console.WriteLine("Game Board at the start:");
            gameBoard.PrintBoard();

            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine($"Iteration {i + 1}:");
                Console.WriteLine("Highlanders' positions:");

                // Print all highlanders' positions
                foreach (var highlander in highlanders)
                {
                    highlander.PrintPosition();
                }

                // Move each highlander
                foreach (var highlander in highlanders)
                {
                    if (highlander.IsAlive)
                    {
                        // Get current position
                        var (currentRow, currentCol) = highlander.GetPosition();

                        // Move highlander
                        highlander.Move(gameBoard);

                        // Get new position
                        var (newRow, newCol) = highlander.GetPosition();

                        // Update board with the highlander
                        if (currentRow != newRow || currentCol != newCol)
                        {
                            // Remove Highlander from old position
                            gameBoard.RemoveItem(highlander, currentRow, currentCol);

                            // Add Highlander to new position
                            gameBoard.AddItem(highlander, newRow, newCol);
                        }
                    }
                }

                // Print the updated board
                gameBoard.PrintBoard();

                // Check if only one highlander is left
                if (highlanders.Count(h => h.IsAlive) <= 1)
                {
                    break;
                }
            }
            DisplayResults();
        }

            private void HandleInteractions(Highlander highlander)
        {
            foreach (var h in highlanders)
            {
                if (h != highlander && h.Position == highlander.Position)
                {
                    highlander.Interact(h);
                    //TODO: Implement logic for interaction outcome
                }
            }
        }

        private void DisplayResults()
        {
            Console.WriteLine("Simulation ended.");
            if (highlanders.Count == 1)
            {
                Console.WriteLine($"Winner: {highlanders[0].Id}");
            }
            else
            {
                Console.WriteLine("Multiple Highlanders survived.");
            }
        }
    }
}
