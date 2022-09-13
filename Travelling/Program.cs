using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using static System.Formats.Asn1.AsnWriter;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

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
            var rDif = goal.Item1 - i >= 2 && goal.Item1 - i > i;
            for (var j = 0; j < columns; j++)
            {
                list.Add(new Tile
                {
                    Path = (i, j),
                    Reward = random.Next(-1, 100),
                    Distance = (Math.Abs(i - goal.Item1), Math.Abs(j - goal.Item2))
                });

                if (!rDif) continue;

                if (goal.Item2 - j >= 2 && goal.Item2 - j > j)
                    list[^1].IsPortal = Convert.ToBoolean(random.Next(2));
            }

            stage.Add(list);
        }


        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            if (stage[i][j].IsPortal)
            {
                stage[i][j].Score = stage[i][j].Reward = 200;
                var rLeft = random.Next(i, goal.Item1 - i);
                var cLeft = random.Next(j, goal.Item2 - j);

                while (rLeft == 2 && cLeft == 2)
                {
                    rLeft = random.Next(i, goal.Item1 - i);
                    cLeft = random.Next(j, goal.Item2 - j);
                }

                stage[i][j].Neighbours.Add(stage[rLeft][cLeft]);
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

        var currentStage = stage[start.Item1][start.Item2];
        var a = GC.GetGeneration(stage);
        stage.Clear();
        GC.Collect(a, GCCollectionMode.Forced);

        if (!FindPath(new List<Tile> { currentStage }, goal))
            Console.WriteLine("No Path Found...\n");
    }

    private static bool FindPath(ICollection<Tile> activeTiles, (int, int) goal)
    {
        var score = 0;
        var visitedTiles = new List<Tile>();

        while (activeTiles.Any())
        {
            var checkTile = activeTiles.OrderBy(c => c.CostDistance).First();
            score += checkTile.Reward;
            checkTile.Score = score;

            if (checkTile.Path == goal)
            {
                //We found the destination and we can be sure (Because the the OrderBy above)
                //That it's the most low cost option. 
                Console.WriteLine("Goal: " + goal + ", Start: " + visitedTiles.First().Path);
                Console.WriteLine("\nRetracing steps backwards...\n");

                do
                {
                    Console.WriteLine("Path: " + checkTile.Path + ", Score: " + checkTile.Score + Environment.NewLine);

                    checkTile = checkTile.Parent;

                } while (checkTile != null);

                return true;
            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);
            checkTile.Neighbours.Remove(visitedTiles.Count > 1 ? visitedTiles[^2] : visitedTiles[^1]);

            var walkableTiles = checkTile.GetWalkableTiles();

            foreach (var walkableTile in walkableTiles.Where(walkableTile =>
                         visitedTiles.All(x => x.Path != walkableTile.Path)))
                //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                if (activeTiles.Any(x => x.Path == walkableTile.Path))
                {
                    var existingTile = activeTiles.First(x => x.Path == walkableTile.Path);

                    if (!(existingTile.CostDistance > checkTile.CostDistance)) continue;

                    activeTiles.Remove(existingTile);
                    activeTiles.Add(walkableTile);
                }
                else
                {
                    //We've never seen this tile before so add it to the list. 
                    activeTiles.Add(walkableTile);
                }
        }

        return false;
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
            var rDif = goal.Item1 - i >= 2 && goal.Item1 - i > i;
            for (var j = 0; j < columns; j++)
                list.Add(new Tile
                {
                    Path = (i, j),
                    Reward = random.Next(-1, 100),
                    Distance = (Math.Abs(i - goal.Item1), Math.Abs(j - goal.Item2)),
                    IsPortal = rDif && goal.Item2 - j >= 2 && goal.Item2 - j > j &&
                               Convert.ToBoolean(random.Next(2))
                });

            stage.Add(list);
        }


        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
        {
            if (stage[i][j].IsPortal)
            {
                stage[i][j].Score = stage[i][j].Reward = 200;
                var rLeft = random.Next(i, goal.Item1 - i);
                var cLeft = random.Next(j, goal.Item2 - j);

                while (rLeft == 2 && cLeft == 2)
                {
                    rLeft = random.Next(i, goal.Item1 - i);
                    cLeft = random.Next(j, goal.Item2 - j);
                }

                stage[i][j].Neighbours.Add(stage[rLeft][cLeft]);
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

        var currentStage = stage[start.Item1][start.Item2];
        var a = GC.GetGeneration(stage);
        stage.Clear();
        GC.Collect(a, GCCollectionMode.Forced);

        if (!FindPath(new List<Tile> { currentStage }, goal))
            Console.WriteLine("No Path Found...\n");
    }
}