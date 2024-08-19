using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander_Components.lander
{
    public static class HighlanderFactory
    {
        public static List<Highlander> generateRandomHighlanders(int count, int rows, int columns)
        {
            Random random = new Random();
            List<Highlander> highlanders = new List<Highlander>();
            List<(int, int)> occupiedPositions = new List<(int, int)>();

            for (int i = 0; i < count; i++)
            {
                int id = i + 1; // Unique ID for each Highlander
                int power = random.Next(1, 101); // Random power between 1 and 100
                int age = random.Next(20, 300); // Random age between 20 and 300
                (int, int) position;

                // Ensure unique position
                do
                {
                    position = (random.Next(rows), random.Next(columns));
                } while (occupiedPositions.Contains(position));

                Highlander highlander;

                // Randomly choose if the Highlander is Good or Bad
                if (random.Next(2) == 0)
                {
                    highlander = new GoodHighlander(id, power, age, position, true);
                }
                else
                {
                    highlander = new BadHighlander(id, power, age, position, true);
                }

                highlanders.Add(highlander);
                occupiedPositions.Add(position);
            }
            return highlanders;
        }

    }
}
