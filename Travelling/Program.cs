#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace Travelling;

public class Program
{
    private static void Main()
    {
        Open();
        StoryMode();
    }

    private static void Open()
    {
        Random random = new();
        var rows = random.Next(2, 100);
        var columns = random.Next(2, 100);
        var stage = new List<List<Tile>>(rows);
        var goal = (random.Next(1, rows - 1), random.Next(1, columns - 1));
        var start = (random.Next(1, rows - 1), random.Next(1, columns - 1));

        for (var i = 0; i < rows; i++)
        {
            var list = new List<Tile>(columns);
            for (var j = 0; j < columns; j++)
                list.Add(new Tile
                {
                    Path = (i, j),
                    Reward = random.Next(-1, 10),
                    Distance = (goal.Item1 - i, goal.Item2 - j)
                });

            stage.Add(list);
        }


        stage[goal.Item1][goal.Item2].Reward = 50;

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            if (j > 0)
            {
                //Left
                stage[i][j].Neighbours.Add(stage[i][j - 1]);

                if (i + 1 < rows) //Up Left
                    stage[i][j].Neighbours.Add(stage[i + 1][j - 1]);
            }

            if (i + 1 < rows)
            {
                //Up
                stage[i][j].Neighbours.Add(stage[i + 1][j]);

                if (j + 1 < columns) //Up Right
                    stage[i][j].Neighbours.Add(stage[i + 1][j + 1]);
            }

            if (j + 1 < columns)
            {
                //Right
                stage[i][j].Neighbours.Add(stage[i][j + 1]);

                if (i - 1 >= 0) //Down Right
                    stage[i][j].Neighbours.Add(stage[i - 1][j + 1]);
            }

            if (i - 1 < 0) continue;
            //Down
            stage[i][j].Neighbours.Add(stage[i - 1][j]);

            if (j > 0) //Down Left
                stage[i][j].Neighbours.Add(stage[i - 1][j - 1]);
        }

        List<Tile> locations = new();
        var currentState = stage[start.Item1][start.Item2];
        var a = GC.GetGeneration(stage);
        stage.Clear();
        GC.Collect(a, GCCollectionMode.Forced);
        locations.Add(currentState);

        while (currentState.Distance != (0, 0)) currentState.Story.NextStage(ref currentState);

        while (currentState.Distance != (0, 0))
        {
            if (currentState.Neighbours.Any(x => x.Distance.Item1 >= 0 || x.Distance.Item2 >= 0))
            {
                if (currentState.Neighbours.Any(x => x.Distance.Item1 >= 0 && x.Distance.Item2 >= 0))
                    currentState = currentState.Neighbours
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 >= 0).MinBy(v => v.Distance);
                else
                    currentState = currentState.Neighbours.Any(x => x.Distance.Item1 >= 0)
                        ? currentState.Neighbours.Where(x => x.Distance.Item2 < 0 && x.Distance.Item1 >= 0)
                            .OrderBy(c => c.Distance.Item1).ThenByDescending(b => b.Distance.Item2).First()
                        : currentState.Neighbours
                            .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 < 0)
                            .OrderByDescending(c => c.Distance.Item1).ThenBy(b => b.Distance.Item2).First();
            }
            else
            {
                currentState = currentState.Neighbours.MaxBy(x => x.Distance);
            }

            locations.Add(currentState);
        }

        var step = 0;

        Console.Write("Goal: " + goal + Environment.NewLine + Environment.NewLine);
        locations.ForEach(x => Console.WriteLine("stage " + step++ + ": " + x.Path + Environment.NewLine));
    }

    private static void StoryMode()
    {
        Random random = new();
        var rows = random.Next(2, 100);
        var columns = random.Next(2, 100);
        var stage = new List<List<Tile>>(rows);
        var goal = (random.Next(1, rows - 1), random.Next(1, columns - 1));
        var start = (random.Next(1, rows - 1), random.Next(1, columns - 1));

        for (var i = 0; i < rows; i++)
        {
            var list = new List<Tile>(columns);
            for (var j = 0; j < columns; j++)
                list.Add(new Tile
                {
                    Path = (i, j),
                    Reward = random.Next(-1, 10),
                    Distance = (goal.Item1 - i, goal.Item2 - j)
                });

            stage.Add(list);
        }


        stage[goal.Item1][goal.Item2].Reward = 50;

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            if (j > 0)
            {
                //Left
                stage[i][j].Neighbours.Add(stage[i][j - 1]);

                if (i + 1 < rows) //Up Left
                    stage[i][j].Neighbours.Add(stage[i + 1][j - 1]);
            }

            if (i + 1 < rows)
            {
                //Up
                stage[i][j].Neighbours.Add(stage[i + 1][j]);

                if (j + 1 < columns) //Up Right
                    stage[i][j].Neighbours.Add(stage[i + 1][j + 1]);
            }

            if (j + 1 < columns)
            {
                //Right
                stage[i][j].Neighbours.Add(stage[i][j + 1]);

                if (i - 1 >= 0) //Down Right
                    stage[i][j].Neighbours.Add(stage[i - 1][j + 1]);
            }

            if (i - 1 >= 0)
            {
                //Down
                stage[i][j].Neighbours.Add(stage[i - 1][j]);

                if (j > 0) //Down Left
                    stage[i][j].Neighbours.Add(stage[i - 1][j - 1]);
            }
        }

        var currentState = stage[start.Item1][start.Item2];
        var a = GC.GetGeneration(stage);
        stage.Clear();
        GC.Collect(a, GCCollectionMode.Forced);

        while (currentState.Distance != (0, 0)) currentState.Story.NextStage(ref currentState);
    }
}