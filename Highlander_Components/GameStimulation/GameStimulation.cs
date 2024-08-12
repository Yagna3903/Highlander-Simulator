using Highlander_Component.GameBoard;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Components.GameStimulation
{
    public class GameStimulation
    {
        // Declare the game board and highlanders
        private IGameBoard gameBoard;
        private List<Highlander> highlanders;


        public GameStimulation(IGameBoard gameBoard, List<Highlander> highlanders)
        {
            this.gameBoard = gameBoard;
            this.highlanders = highlanders;
            GameStart.PlaceHighlanders(this.gameBoard, this.highlanders);
        }


        public void RunSimulation(int maxIterations)
        {
            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine($"Iteration {i + 1}:");
                Console.WriteLine("Highlanders' positions:");

                foreach (var highlander in highlanders)
                {
                    highlander.PrintPosition();
                    highlander.Move(gameBoard);
                    HandleInteractions(highlander);
                }
                gameBoard.UpdateBoard(highlanders);
                gameBoard.PrintBoard();

                // If only 
                if (highlanders.Count <= 1)
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
