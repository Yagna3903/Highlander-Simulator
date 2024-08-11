using Highlander_Console.lander;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander_Console.GameLogic
{
    public class GameStimulation
    {
        private IGameBoard gameBoard;
        private List<Highlander> highlanders;

        public GameStimulation(IGameBoard gameBoard, List<Highlander> highlanders)
        {
            this.gameBoard = gameBoard;
            this.highlanders = highlanders;
        }

        public void RunSimulation(int maxIterations)
        {
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                Console.WriteLine($"Iteration {iteration + 1}:");
                foreach (var highlander in highlanders)
                {
                    highlander.Move(gameBoard);
                    HandleInteractions(highlander);
                }

                gameBoard.PrintBoard();

                // Check for end conditions
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
