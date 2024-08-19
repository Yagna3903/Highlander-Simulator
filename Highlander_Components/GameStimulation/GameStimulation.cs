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
        int totalIteration = 0;


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
            gameBoard.Id = HighlanderRepository.AddRecord(null, 0,0,0,0);

            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine($"Iteration {i + 1}:");
                Console.WriteLine("Highlanders' positions:");

                // every 365 iterations, highlanders age by 1 year
                if (i % 365 == 0)
                {
                    foreach (Highlander highlander in highlanders)
                    {
                        if (highlander.IsAlive) { highlander.age(); }
                        // TODO: add database method to update highlander age

                    }
                }

                // Print all highlanders' positions
                //foreach (Highlander highlander in highlanders)
                //{
                //    if (highlander.IsAlive)
                //    {
                //        highlander.PrintPosition();
                //    }
                //}

                Console.WriteLine("Before the move");
                // Print the updated board
                gameBoard.PrintBoard();

                // Move each highlander
                foreach (Highlander highlander in highlanders)
                {
                    if (highlander.IsAlive)
                    {
                        // Get current position
                        var (currentRow, currentCol) = highlander.GetPosition();

                        // Move highlander
                        highlander.Move(gameBoard);

                        // Get new position
                        var (newRow, newCol) = highlander.GetPosition();

                        // Remove Highlander from old position
                        gameBoard.RemoveItem(highlander, currentRow, currentCol);

                        // Add Highlander to new position
                        gameBoard.AddItem(highlander, newRow, newCol);

                    }
                }
                Console.WriteLine("After Move");

                // Print the updated board
                gameBoard.PrintBoard();

                // loop through the game board and handle interaction between highlanders
                for (int row = 0; row < gameBoard.Rows; row++)
                {
                    for (int col = 0; col < gameBoard.Columns; col++)
                    {
                        if (gameBoard.Board[row, col].Count > 1)
                        {
                            // all highlanders at current position
                            List<Highlander> highlanders = gameBoard.Board[row, col];
                            for (int j = 0; j < highlanders.Count - 1; j++)
                            {
                                for (int k = j + 1; k < highlanders.Count; k++)
                                {
                                    highlanders[j].Fight(highlanders[k], totalIteration);
                                }
                            }
                        }
                    }
                }

                foreach (Highlander highlander in highlanders)
                {
                    if (!highlander.IsAlive)
                    {
                        gameBoard.RemoveItem(highlander, highlander.Position.Item1, highlander.Position.Item2);
                    }
                }

                Console.WriteLine("After Fight");

                // Print the updated board
                gameBoard.PrintBoard();

                // Check if only one highlander is left
                if (highlanders.Count(h => h.IsAlive) <= 1)
                {
                    break;
                }

                totalIteration++;
            }
            DisplayResults();
        }
        public void RunSimulationStep()
        {
            // Every 365 iterations, highlanders age by 1 year
            if (totalIteration % 365 == 0)
            {
                foreach (Highlander highlander in highlanders)
                {
                    if (highlander.IsAlive) { highlander.age(); }
                }
            }

            // Move each highlander
            foreach (Highlander highlander in highlanders)
            {
                if (highlander.IsAlive)
                {
                    // Get current position
                    var (currentRow, currentCol) = highlander.GetPosition();

                    // Move highlander
                    highlander.Move(gameBoard);

                    // Get new position
                    var (newRow, newCol) = highlander.GetPosition();

                    // Remove Highlander from old position
                    gameBoard.RemoveItem(highlander, currentRow, currentCol);

                    // Add Highlander to new position
                    gameBoard.AddItem(highlander, newRow, newCol);
                }
            }

            // Handle interactions between highlanders
            for (int row = 0; row < gameBoard.Rows; row++)
            {
                for (int col = 0; col < gameBoard.Columns; col++)
                {
                    if (gameBoard.Board[row, col].Count > 1)
                    {
                        // All highlanders at current position
                        List<Highlander> highlandersAtPosition = gameBoard.Board[row, col];

                        for (int j = 0; j < highlandersAtPosition.Count - 1; j++)
                        {
                            for (int k = j + 1; k < highlandersAtPosition.Count; k++)
                            {
                                highlandersAtPosition[j].Fight(highlandersAtPosition[k], totalIteration);

                            }
                        }
                    }
                }
            }

            // Remove dead highlanders from the board
            foreach (Highlander highlander in highlanders)
            {
                if (!highlander.IsAlive)
                {
                    gameBoard.RemoveItem(highlander, highlander.Position.Item1, highlander.Position.Item2);
                }
            }

            // Check if only one highlander is left
            if (highlanders.Count(h => h.IsAlive) <= 1)
            {
                // Optional: Handle game over condition if needed
            }

            totalIteration++;
        }

        // TODO: Implement logic for Highlander interactions in the future
        private void HandleInteractions(Highlander highlander)
        {
            foreach (Highlander h in highlanders)
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
            int killedHighlander = 0;
            int goodHighlander = 0;
            int badHighlander = 0;
            int totalPower = 0;
            int remainingHighlanders = 0;
            string winner = ""; 
            List<Highlander> remainingH = new List<Highlander>();
            foreach (Highlander h in highlanders)
            {
                totalPower+= h.Power;
                if (h is GoodHighlander ) { goodHighlander++; }
                else { badHighlander++; }
                if (h.IsAlive)
                {
                    remainingHighlanders++;
                    remainingH.Add(h);
                }
                else
                {
                    killedHighlander++;
                }
            }
            
            Console.WriteLine("Simulation ended.");
            if (remainingHighlanders == 1)
            {
                winner = remainingH[0] is GoodHighlander ? "good highlander" : "bad highlander";
                Console.WriteLine($"Winner: {remainingH[0].Id}");
            }
            else
            {
                winner = "non";
                Console.WriteLine("Multiple Highlanders survived.");
            }
            HighlanderRepository.UpdateRecord(gameBoard.Id, winner, killedHighlander, totalPower, goodHighlander, badHighlander);
        }
    }
}