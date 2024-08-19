using Highlander_Component.GameBoard;
using System;
using System.ComponentModel;
using System.Data;

namespace Highlander_Components.lander
{
    public abstract class Highlander
    {
        public int Id { get; set; }
        public int Power { get; set; }
        public int Age { get; set; }
        public (int, int) Position { get; set; }
        public bool IsAlive { get; set; }

        private (int, int) currPos;

        protected Random random = new Random();

        // Move the highlander to a new position
        public void Move(IGameBoard<Highlander> gameBoard)
        {
            bool moved = false; // Flag to check if the highlander has moved

            // Possible moves (8 directions)
            var directions = new (int RowOffset, int ColOffset)[]
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),          (0, 1),
                (1, -1), (1, 0), (1, 1)
            };

            currPos = this.Position;

            // Keep trying to move until a valid move is made
            while (!moved)
            {

                // Randomly select a direction from the possible directions
                var direction = directions[random.Next(directions.Length)];

                // Calculate new position
                var newRow = Position.Item1 + direction.RowOffset;
                var newCol = Position.Item2 + direction.ColOffset;

                // Check if the new position is valid

                if (gameBoard.IsPositionValid(newRow, newCol))
                {
                    // Remove Highlander from old position
                    gameBoard.RemoveItem(this, currPos.Item1, currPos.Item2);

                    // Update the position of the Highlander
                    Position = (newRow, newCol);

                    // Print the new position
                    PrintPosition();

                    // Add Highlander to new position
                    gameBoard.AddItem(this, newRow, newCol);

                    moved = true;

                }


            }

        }

        public (int, int) GetPosition()
        {
            return Position;
        }

        public void Fight(Highlander highlander, int iteration)
        {
            // Ensure both Highlanders are alive
            if (!this.IsAlive || !highlander.IsAlive) return;



            // Calculate the weighted probability for each Highlander based on power and age
            double thisHighlanderWeight = (this.Power * 0.6) + (this.Age * 0.4); // 60% weight to power, 40% to age
            double opponentHighlanderWeight = (highlander.Power * 0.6) + (highlander.Age * 0.4); // Same weighting for opponent

            // Calculate the total weight to determine probabilities
            double totalWeight = thisHighlanderWeight + opponentHighlanderWeight;

            // Calculate the winning probability for this Highlander
            double thisHighlanderChance = thisHighlanderWeight / totalWeight;

            // Generate a random number between 0 and 1
            Random random = new Random();
            double fightOutcome = random.NextDouble();


            if (fightOutcome < thisHighlanderChance)
            {
                // This Highlander wins
                Console.WriteLine($"Highlander {this.Id} wins against Highlander {highlander.Id}.");
                this.absorbPower(highlander);
                highlander.Die();
                //TODO DB.updateKill(iteration, id, hishlander.id)
            }
            else
            {
                // Opponent wins
                Console.WriteLine($"Highlander {highlander.Id} wins against Highlander {this.Id}.");
                highlander.absorbPower(this);
                this.Die();
            }
        }


        public void PrintPosition()
        {
            Console.WriteLine($"Highlander {Id} is at position {Position}");
        }
        public abstract void Interact(Highlander highlander, int iteration, IGameBoard<Highlander> gb);

        // Highlander dies
        public void Die()
        {
            IsAlive = false;
        }

        public void absorbPower(Highlander highlander)
        {
            Power += highlander.Power;
        }

        public void age()
        {
            Age++;
        }


    }

    public class GoodHighlander : Highlander
    {
        public GoodHighlander(int Id, int power, int age, (int, int) position, bool IsAlive)
        {
            this.Id = Id;
            Power = power;
            Age = age;
            Position = position;
            this.IsAlive = IsAlive;
        }

        public override void Interact(Highlander highlander, int iteration, IGameBoard<Highlander> gb)
        {
            if (highlander is BadHighlander)
            {
                bool escaped = TryToEscape();
                if (escaped)
                {
                    Move(gb); // Move to a new position
                    Console.WriteLine($"Highlander {Id} escaped from Highlander {highlander.Id}.");
                    return;
                }
                else

                {
                    Fight(highlander, iteration);
                }
            }

            bool TryToEscape()
            {
                return random.NextDouble() < 0.65; // 65% chance of escaping
            }
        }
    }
    public class BadHighlander : Highlander
    {
        public BadHighlander(int Id, int power, int age, (int, int) position, bool IsAlive)
        {
            this.Id = Id;
            Power = power;
            Age = age;
            Position = position;
            this.IsAlive = IsAlive;
        }

        public override void Interact(Highlander highlander, int iteration, IGameBoard<Highlander> gb)
        {
            Fight(highlander, iteration);
        }
    }
}

