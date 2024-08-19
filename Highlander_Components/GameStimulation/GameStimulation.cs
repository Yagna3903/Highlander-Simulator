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

                Console.WriteLine("Before the move");
                gameBoard.PrintBoard();

                HighlanderAgeMove();


                Console.WriteLine("After Move");

                // Print the updated board
                gameBoard.PrintBoard();



                //Console.WriteLine("After Fight");

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
            HighlanderAgeMove();



            // Check if only one highlander is left
            if (highlanders.Count(h => h.IsAlive) <= 1)
            {
                // Optional: Handle game over condition if needed
            }

            totalIteration++;
        }

        private void HighlanderAgeMove()
        {

            foreach (Highlander highlander in highlanders)
            {
                if (highlander.IsAlive)
                {
                    // every 365 iterations, highlanders age by 1 year
                    if (totalIteration % 365 == 0)
                    {
                        highlander.age();
                    }

                    highlander.Move(gameBoard);
                }

            }
        }

        public void HandleInteractions()
        {
            bool continueInteractions = true;

            while (continueInteractions)
            {
                continueInteractions = false;

                for (int row = 0; row < gameBoard.Rows; row++)
                {
                    for (int col = 0; col < gameBoard.Columns; col++)
                    {
                        if (gameBoard.Board[row, col].Count > 1)
                        {
                            List<Highlander> highlandersAtPosition = gameBoard.Board[row, col];

                            // Separate good and bad Highlanders to handle interactions accordingly
                            var goodHighlanders = highlandersAtPosition.Where(h => h is GoodHighlander).ToList();
                            var badHighlanders = highlandersAtPosition.Where(h => h is BadHighlander).ToList();

                            // Only handle interactions if there are bad Highlanders present
                            if (badHighlanders.Count > 0)
                            {
                                foreach (var goodHighlander in goodHighlanders)
                                {
                                    foreach (var badHighlander in badHighlanders)
                                    {
                                        goodHighlander.Interact(badHighlander, totalIteration, gameBoard);
                                        continueInteractions = true;
                                    }
                                }

                                // Handle interactions between bad Highlanders
                                for (int j = 0; j < badHighlanders.Count - 1; j++)
                                {
                                    for (int k = j + 1; k < badHighlanders.Count; k++)
                                    {
                                        badHighlanders[j].Interact(badHighlanders[k], totalIteration, gameBoard);
                                        continueInteractions = true;
                                    }
                                }
                            }
                        }
                    }
                }

                RemoveDeadHighlanders();

                // Check if only good Highlanders remain or if only one Highlander is left
                var aliveHighlanders = highlanders.Where(h => h.IsAlive).ToList();
                if (aliveHighlanders.Count <= 1 || aliveHighlanders.All(h => h is GoodHighlander))
                {
                    continueInteractions = false;
                }
            }
        }

        private void RemoveDeadHighlanders()
        {
            foreach (Highlander highlander in highlanders)
            {
                if (!highlander.IsAlive)
                {
                    gameBoard.RemoveItem(highlander, highlander.Position.Item1, highlander.Position.Item2);
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