using Highlander_Components.lander;
using System;
using System.Collections.Generic;

namespace Highlander_Components.Lander
{
    public static class HighlanderFactory
    {
        public static List<Highlander> GenerateRandomHighlanders(int count, int rows, int columns)
        {
            Random random = new Random();
            List<Highlander> highlanders = new List<Highlander>();
            HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();

            for (int i = 0; i < count; i++)
            {
                int id = i + 1; // Unique ID for each Highlander
                int power = random.Next(1, 101); // Random power between 1 and 100
                int age = random.Next(20, 300); // Random age between 20 and 300
                (int, int) position;

                // Ensure unique position by checking against the occupied positions set
                do
                {
                    position = (random.Next(rows), random.Next(columns));
                } while (!occupiedPositions.Add(position));

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
            }

            return highlanders;
        }

        public static Highlander GenerateSingleHighlander(int rows, int columns)
        {
            Random random = new Random();
            int id = random.Next(1000, 9999); // Generate a unique ID (or you can implement a better ID generation strategy)
            int power = random.Next(1, 101); // Random power between 1 and 100
            int age = random.Next(20, 300); // Random age between 20 and 300
            (int, int) position = (random.Next(rows), random.Next(columns));

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

            return highlander;
        }
    }
}