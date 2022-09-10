namespace Travelling;

public class Program
{
    private static void Main()
    {
        Open();
       // StoryMode();
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
                    Reward = random.Next(-1, 100),
                    Distance = (goal.Item1 - i, goal.Item2 - j),
                    IsPortal = goal.Item1 - i >= 2 && goal.Item1 - i > i && goal.Item2 - j >= 2 && goal.Item2 - j > j &&
                               Convert.ToBoolean(random.Next(2))
                });

            stage.Add(list);
        }


        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            if (stage[i][j].IsPortal)
            {
                stage[i][j].Reward += 200;
                var rLeft = random.Next(i, goal.Item1 - i);
                var cLeft = random.Next(j, goal.Item2 - j);

                while (rLeft == 2 && cLeft == 2)
                {
                    rLeft = random.Next(i, goal.Item1 - i);
                    cLeft = random.Next(j, goal.Item2 - j);
                }

                stage[i][j].Neighbours.Add(stage[random.Next(i, goal.Item1 - i)][random.Next(j, goal.Item2 - j)]);
            }

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
        var currentStage = stage[start.Item1][start.Item2];
        var a = GC.GetGeneration(stage);
        stage.Clear();
        GC.Collect(a, GCCollectionMode.Forced);
        locations.Add(currentStage);

        while (currentStage.Distance != (0, 0))
        {
            var score = currentStage.Score;
            currentStage = currentStage.Neighbours.MinByDistance();
            score += currentStage.Reward;
            currentStage.Score = score;
            locations.Add(currentStage);
        }

        var step = 0;

        Console.Write("Goal: " + goal + Environment.NewLine + Environment.NewLine);
        locations.ForEach(x =>
            Console.WriteLine("Stage " + step++ + ": " + x.Path + ", Score: " + x.Score + ", IsPortal: " + x.IsPortal +
                              Environment.NewLine));
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

            if (i - 1 < 0) continue;
            //Down
            stage[i][j].Neighbours.Add(stage[i - 1][j]);

            if (j > 0) //Down Left
                stage[i][j].Neighbours.Add(stage[i - 1][j - 1]);
        }

        var currentStage = stage[start.Item1][start.Item2];
        var a = GC.GetGeneration(stage);
        stage.Clear();
        GC.Collect(a, GCCollectionMode.Forced);

        while (currentStage.Distance != (0, 0)) currentStage = currentStage.Story.NextStage(currentStage.Reward);
    }
}