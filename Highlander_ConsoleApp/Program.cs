using Highlander_Component.GameBoard;
using Highlander_Components.GameStimulation;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                // Initialize the game board
                IGameBoard gameBoard = new GameBoard(5, 5);

                // Create Highlanders
                List<Highlander> highlanders = new List<Highlander>  {
                    new GoodHighlander(Id: 1, power: 10, age: 100, position: (0, 0), IsAlive: true),
                    new BadHighlander(Id: 2, power: 15, age: 150, position: (1, 1), IsAlive: true)
                };

                // Initialize the simulation manager
                GameStimulation gamePlay = new GameStimulation(gameBoard, highlanders);

                // Run the simulation
                gamePlay.RunSimulation(5);

                Console.ReadLine();
            }
        }
    }
}
