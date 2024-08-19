using Highlander_Component.Database;  // Added for database access
using Highlander_Components.Database;
using Highlander_Component.GameBoard;
using Highlander_Components.GameStimulation;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;
using Highlander_Components.Lander;

namespace Highlander_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize DatabaseContext and HighlanderRepository
            DatabaseContext dbContext = new DatabaseContext();
            HighlanderRepository repo = new HighlanderRepository(dbContext.GetConnection().ConnectionString);

            // Attempt to fetch existing Highlanders from the database
            List<Highlander> highlanders = repo.GetAllHighlanders();

            // If no Highlanders exist in the database, create new ones
            if (highlanders.Count == 0)
            {
                // Declare variables
                int rows = 5;
                int columns = 5;
                int maxIterations = 20;
                int highlanderCount = 0;
                int totalCells = rows * columns;
                Random r = new Random();

                // Randomly generate the number of highlanders
                highlanderCount = r.Next(totalCells / 2, totalCells);

                // Initialize the game board
                IGameBoard<Highlander> gameBoard1 = new GameBoard<Highlander>(rows, columns);

                // Create Highlanders
                highlanders = HighlanderFactory.GenerateRandomHighlanders(highlanderCount, gameBoard1.Rows, gameBoard1.Columns);

                // Save the generated Highlanders to the database
                repo.SaveHighlanders(highlanders);
            }

            // Initialize the simulation manager
            IGameBoard<Highlander> gameBoard = new GameBoard<Highlander>(5, 5);
            GameStimulation gamePlay = new GameStimulation(gameBoard, highlanders);

            // Run the simulation
            gamePlay.RunSimulation(20);

            // Update the database with the simulation results
            foreach (var highlander in highlanders)
            {
                repo.UpdateHighlander(highlander);
            }

            Console.ReadLine();
        }
    }
}