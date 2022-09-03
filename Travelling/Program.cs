using System.Collections;
using System.Collections.Generic;

namespace Travelling
{
    public class Program
    {
        static void Main()
        {
            Random random = new();
            var rows = random.Next(1, 100);
            var columns = random.Next(1, 100);
            var Stage = new List<List<Tile>>(rows);
            var goal = (random.Next(1, rows) - 1, random.Next(1, columns) - 1);
            var start = (random.Next(1, rows) - 1, random.Next(1, columns) - 1);

            for (int i = 0; i < rows; i++)
            {
                var list = new List<Tile>();
                for (int j = 0; j < columns; j++)
                {
                    list.Add(new Tile()
                    {
                        Path = (i, j),
                        Reward = random.Next(-1, 10),
                        Distance = (goal.Item1 - i, goal.Item2 - j)
                    });
                }
                Stage.Add(list);
            }


            Stage[goal.Item1][goal.Item2].Reward = 50;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (j > 0) //Left
                        Stage[i][j].Neighbours.Add(Stage[i][j - 1]);

                    if (((i + 1) < rows) & (j > 0)) //Up Left
                        Stage[i][j].Neighbours.Add(Stage[i + 1][j - 1]);

                    if ((i + 1) < rows) //Up
                        Stage[i][j].Neighbours.Add(Stage[i + 1][j]);

                    if (((i + 1) < rows) & ((j + 1) < columns)) //Up Right
                        Stage[i][j].Neighbours.Add(Stage[i + 1][j + 1]);

                    if ((j + 1) < columns) //Right
                        Stage[i][j].Neighbours.Add(Stage[i][j + 1]);

                    if (((i - 1) > 0) & ((j + 1) < columns)) //Down Right
                        Stage[i][j].Neighbours.Add(Stage[i - 1][j + 1]);

                    if ((i - 1) > 0) //Down
                        Stage[i][j].Neighbours.Add(Stage[i - 1][j]);

                    if (((i - 1) > 0) & (j > 0)) //Down Left
                        Stage[i][j].Neighbours.Add(Stage[i - 1][j - 1]);
                }
            }

            List<Tile> locations = new();
            Tile currentState = Stage[start.Item1][start.Item2];

            locations.Add(currentState);

            while (currentState.Distance != (0, 0))
            {
                var list = currentState.Neighbours;
                //currentState = currentState.Neighbours.MinBy(v => v.Distance);
                if (currentState.Neighbours.Any(x => x.Distance.Item1 >= 0 || x.Distance.Item2 >= 0))
                {
                    if (currentState.Neighbours.Any(x => x.Distance.Item1 >= 0 && x.Distance.Item2 >= 0))
                        currentState = currentState.Neighbours.Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 >= 0).OrderBy(v => v.Distance.Item1).ThenBy(c => c.Distance.Item2).First();
                    else
                    {
                        if (currentState.Neighbours.Any(x => x.Distance.Item1 >= 0))
                            currentState = currentState.Neighbours.Where(x => x.Distance.Item2 < 0 && x.Distance.Item1 >= 0).OrderBy(c => c.Distance.Item1).ThenByDescending(b => b.Distance.Item2).First();
                        else
                            currentState = currentState.Neighbours.Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 < 0).OrderByDescending(c => c.Distance.Item1).ThenBy(b => b.Distance.Item2).First();
                    }
                }
                else
                    currentState=list.OrderByDescending(x => x.Distance).First();
                locations.Add(currentState);
            }

            int step = 0;

            Console.WriteLine("Goal: " + goal.ToString() + Environment.NewLine);
            locations.ForEach(x => Console.WriteLine("Stage " + (step++).ToString() + ": " + x.Path.ToString() + Environment.NewLine));
        }

    } // for
} // Train
