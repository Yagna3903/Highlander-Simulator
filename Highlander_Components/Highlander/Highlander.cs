using Highlander_Component.GameBoard;
using System;

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
            Console.WriteLine($"Highlander {Id} is fighting Highlander {highlander.Id}");
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
