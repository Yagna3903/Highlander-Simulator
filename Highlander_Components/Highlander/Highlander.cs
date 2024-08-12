using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander_Console.lander
{
    public abstract class Highlander
    {
        public int Id { get; set; }
        public int Power { get; set; }
        public int Age { get; set; }
        public (int, int) Position { get; set; }
        public bool IsAlive { get; set; }

        Random random = new Random();

        public void Move(IGameBoard gameBoard)
        {
            bool moved = false;

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
                    Console.WriteLine($"Highlander {Id} moved to position ({newRow}, {newCol})");
                    moved = true;
                }
            }

        }
        public abstract void Interact(Highlander highlander);
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
