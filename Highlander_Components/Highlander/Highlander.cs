using Highlander_Component.GameBoard;
using System;
using System.ComponentModel;

namespace Highlander_Components.lander
{
    public abstract class Highlander
    {
        public int Id { get; set; }
        public int Power { get; set; }
        public int Age { get; set; }
        public (int, int) Position { get; set; }
        public bool IsAlive { get; set; }

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
                    Position = (newRow, newCol);
                    PrintPosition();
                    moved = true;
                }
            }

        }

        public (int, int) GetPosition()
        {
            return Position;
        }

        public void Fight(Highlander highlander)
        {
            // Ensure both Highlanders are alive
            if (!this.IsAlive || !highlander.IsAlive) return;

            // Calculate the probability of winning based on power
            double totalPower = this.Power + highlander.Power;
            double thisHighlanderChance = this.Power / totalPower;

            // Generate a random number between 0 and 1
            Random random = new Random();
            double fightOutcome = random.NextDouble();

            if (fightOutcome < thisHighlanderChance)
            {
                // This Highlander wins
                Console.WriteLine($"Highlander {this.Id} wins against Highlander {highlander.Id}.");
                this.absorbPower(highlander);
                highlander.Die();
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
        public abstract void Interact(Highlander highlander);

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

        public override void Interact(Highlander highlander)
        {
            if (highlander is BadHighlander)
            {
                // TODO: Implement logic for interaction between GoodHighlander and BadHighlander
            }
        }

        private bool TryToEscape()
        {
            return random.Next(2) == 0;
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

        public override void Interact(Highlander highlander)
        {
            // TODO : Implement this method
        }

    }
}
