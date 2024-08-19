using Highlander_Component.GameBoard;
using Highlander_Components.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Highlander_Component.Database;
using Highlander_Components.lander;
using Highlander_Components.Lander;

namespace Highlander_Components.GameStimulation
{
    public class GameStimulation
    {
        // Declare the game board and highlanders
        private IGameBoard<Highlander> gameBoard;
        private List<Highlander> highlanders;
        private HighlanderRepository repo;
        private int totalIteration = 0;

        public GameStimulation(IGameBoard<Highlander> gameBoard, List<Highlander> highlanders, HighlanderRepository repo)
        {
            this.gameBoard = gameBoard;
            this.highlanders = highlanders;
            this.repo = repo;
            GameStart.PlaceHighlanders(this.gameBoard, this.highlanders);
        }

        public void RunSimulationWithCRUD(int maxIterations)
        {
            Console.WriteLine("Game Board at the start:");
            gameBoard.PrintBoard();

            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine($"Iteration {i + 1}:");
                Console.WriteLine("Highlanders' positions:");

                // Handle CRUD operations
                HandleCRUDOperations();

                // Aging logic
                if (i % 365 == 0)
                {
                    foreach (Highlander highlander in highlanders)
                    {
                        if (highlander.IsAlive) { highlander.age(); }
                    }
                }

                Console.WriteLine("Before the move");
                gameBoard.PrintBoard();

                // Move each highlander
                foreach (Highlander highlander in highlanders)
                {
                    if (highlander.IsAlive)
                    {
                        var (currentRow, currentCol) = highlander.GetPosition();
                        highlander.Move(gameBoard);
                        var (newRow, newCol) = highlander.GetPosition();
                        gameBoard.RemoveItem(highlander, currentRow, currentCol);
                        gameBoard.AddItem(highlander, newRow, newCol);
                    }
                }

                Console.WriteLine("After Move");
                gameBoard.PrintBoard();

                // Interaction and combat logic
                HandleHighlanderInteractions();

                Console.WriteLine("After Fight");
                gameBoard.PrintBoard();

                // Update the database after each iteration
                foreach (var highlander in highlanders)
                {
                    repo.UpdateHighlander(highlander);
                }

                // Check if only one highlander is left
                if (highlanders.Count(h => h.IsAlive) <= 1)
                {
                    break;
                }

                totalIteration++;
            }
            DisplayResults();
        }

        private void HandleCRUDOperations()
        {
            Console.WriteLine("Choose an operation: 1-Create, 2-Read, 3-Update, 4-Delete, 5-Continue");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateHighlander();
                    break;
                case "2":
                    ReadHighlander();
                    break;
                case "3":
                    UpdateHighlander();
                    break;
                case "4":
                    DeleteHighlander();
                    break;
                case "5":
                    // Continue with the simulation
                    break;
                default:
                    Console.WriteLine("Invalid choice, continuing with the simulation.");
                    break;
            }
        }

        private void CreateHighlander()
        {
            Console.WriteLine("Creating a new Highlander...");
            // Assuming we have a method to generate a new Highlander
            Highlander newHighlander = HighlanderFactory.GenerateSingleHighlander(gameBoard.Rows, gameBoard.Columns);
            highlanders.Add(newHighlander);
            gameBoard.AddItem(newHighlander, newHighlander.Position.Item1, newHighlander.Position.Item2);
            repo.SaveHighlanders(new List<Highlander> { newHighlander });
            Console.WriteLine("New Highlander created and added to the simulation.");
        }

        private void ReadHighlander()
        {
            Console.WriteLine("Enter the ID of the Highlander to view:");
            int id = int.Parse(Console.ReadLine());
            Highlander highlander = highlanders.FirstOrDefault(h => h.Id == id);

            if (highlander != null)
            {
                Console.WriteLine($"Highlander ID: {highlander.Id}, Power: {highlander.Power}, Age: {highlander.Age}, Position: ({highlander.Position.Item1}, {highlander.Position.Item2}), Alive: {highlander.IsAlive}");
            }
            else
            {
                Console.WriteLine("Highlander not found.");
            }
        }

        private void UpdateHighlander()
        {
            Console.WriteLine("Enter the ID of the Highlander to update:");
            int id = int.Parse(Console.ReadLine());
            Highlander highlander = highlanders.FirstOrDefault(h => h.Id == id);

            if (highlander != null)
            {
                Console.WriteLine("Enter new power value:");
                highlander.Power = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter new age value:");
                highlander.Age = int.Parse(Console.ReadLine());

                repo.UpdateHighlander(highlander);
                Console.WriteLine("Highlander updated.");
            }
            else
            {
                Console.WriteLine("Highlander not found.");
            }
        }

        private void DeleteHighlander()
        {
            Console.WriteLine("Enter the ID of the Highlander to delete:");
            int id = int.Parse(Console.ReadLine());
            Highlander highlander = highlanders.FirstOrDefault(h => h.Id == id);

            if (highlander != null)
            {
                highlanders.Remove(highlander);
                gameBoard.RemoveItem(highlander, highlander.Position.Item1, highlander.Position.Item2);
                repo.DeleteHighlander(id);
                Console.WriteLine("Highlander deleted.");
            }
            else
            {
                Console.WriteLine("Highlander not found.");
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

        private void HandleInteractions(Highlander highlander)
        {
            foreach (Highlander h in highlanders)
            {
                if (!highlander.IsAlive)
                {
                    gameBoard.RemoveItem(highlander, highlander.Position.Item1, highlander.Position.Item2);
                }
            }
        }

        private void DisplayResults()
        {
            int remainingHighlanders = 0;
            List<Highlander> remainingH = new List<Highlander>();
            foreach (Highlander h in highlanders)
            {
                if (h.IsAlive)
                {
                    remainingHighlanders++;
                    remainingH.Add(h);
                }
            }

            Console.WriteLine("Simulation ended.");
            if (remainingHighlanders == 1)
            {
                Console.WriteLine($"Winner: {remainingH[0].Id}");
            }
            else
            {
                Console.WriteLine("Multiple Highlanders survived.");
            }
        }
    }
}