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

        public abstract void Move(IGameBoard gameBoard);
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

        public override void Move(IGameBoard gameBoard)
        {
            // TODO : Implement  method to move highlander on the game board
            
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

        public override void Move(IGameBoard gameBoard)
        {
            // TODO : Implement this method
            
        }
    }
}
