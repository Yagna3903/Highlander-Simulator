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

            //Declare variables
            int rows = 5;
            int columns = 5;
            int maxIterations = 50;
            int highlanderCount = 0;
            int totalCells = rows * columns;
            Random r = new Random();

            // Randomly generate the number of highlanders
            highlanderCount = r.Next(totalCells/2, totalCells);

            // Initialize the game board
            IGameBoard<Highlander> gameBoard = new GameBoard<Highlander>(rows, columns);

            // Create Highlanders
            List<Highlander> highlanders = HighlanderFactory.generateRandomHighlanders(highlanderCount, gameBoard.Rows, gameBoard.Columns);

            // Initialize the simulation manager
            GameStimulation gamePlay = new GameStimulation(gameBoard, highlanders);

            // Run the simulation
            gamePlay.RunSimulation(maxIterations);

            Console.ReadLine();

        }
    }
}
