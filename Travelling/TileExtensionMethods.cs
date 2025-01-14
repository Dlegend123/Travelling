﻿#pragma warning disable CS8603 // Possible null reference argument.

using System.Runtime.InteropServices;

namespace Travelling
{
    internal static class TileExtensionMethods
    {
        internal static List<Tile> SortTiles(this IReadOnlyCollection<Tile> tiles)
        {
            if (!tiles.Any(x => x.Distance.Item1 >= 0 || x.Distance.Item2 >= 0))
                return tiles.OrderByDescending(x => x.Distance).ToList();
            {
                if (tiles.Any(x => x.Distance.Item1 >= 0 && x.Distance.Item2 >= 0))
                    return tiles
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 >= 0).OrderBy(v => v.Distance).ToList();
                return tiles.Any(x => x.Distance.Item1 >= 0)
                    ? tiles.Where(x => x.Distance.Item2 < 0 && x.Distance.Item1 >= 0)
                        .OrderBy(c => c.Distance.Item1).ThenByDescending(b => b.Distance.Item2).ToList()
                    : tiles
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 < 0)
                        .OrderByDescending(c => c.Distance.Item1).ThenBy(b => b.Distance.Item2).ToList();
            }

        }

        internal static List<Tile> GetWalkableTiles(this Tile currentTile)
        {
            var tileSpan = CollectionsMarshal.AsSpan(currentTile.Neighbours);
            foreach (var tile in tileSpan)
            {
                tile.Parent = currentTile;
                tile.Cost = currentTile.Cost + 1;
                tile.Neighbours.Remove(currentTile);
            }

            return currentTile.Neighbours;
        }

        internal static Tile MinByDistance(this List<Tile> tiles)
        {
            if (!tiles.Any(x => x.Distance.Item1 >= 0 || x.Distance.Item2 >= 0))
                return tiles.MaxBy(x => x.Distance);
            {
                if (tiles.Any(x => x.Distance.Item1 >= 0 && x.Distance.Item2 >= 0))
                    return tiles
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 >= 0).MinBy(v => v.Distance);
                return tiles.Any(x => x.Distance.Item1 >= 0)
                    ? tiles.Where(x => x.Distance.Item2 < 0 && x.Distance.Item1 >= 0)
                        .OrderBy(c => c.Distance.Item1).ThenByDescending(b => b.Distance.Item2).First()
                    : tiles
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 < 0)
                        .OrderByDescending(c => c.Distance.Item1).ThenBy(b => b.Distance.Item2).First();
            }

        }

        internal static Tile MaxByDistance(this List<Tile> tiles)
        {
            if (!tiles.Any(x => x.Distance.Item1 >= 0 || x.Distance.Item2 >= 0))
                return tiles.MinBy(x => x.Distance);
            {
                if (tiles.Any(x => x.Distance.Item1 >= 0 && x.Distance.Item2 >= 0))
                    return tiles
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 >= 0).MaxBy(v => v.Distance);
                return tiles.Any(x => x.Distance.Item1 >= 0)
                    ? tiles.Where(x => x.Distance.Item2 < 0 && x.Distance.Item1 >= 0)
                        .OrderByDescending(c => c.Distance.Item1).ThenBy(b => b.Distance.Item2).First()
                    : tiles
                        .Where(x => x.Distance.Item2 >= 0 && x.Distance.Item1 < 0)
                        .OrderBy(c => c.Distance.Item1).ThenByDescending(b => b.Distance.Item2).First();
            }

        }

        internal static Tile MinByReward(this List<Tile> tiles)
        {
            if (!tiles.Any(x => x.Reward >= 0))
                return tiles.MaxBy(x => x.Reward);

            return tiles
                .Where(x => x.Reward >= 0).MinBy(v => v.Reward);
        }
        internal static Tile MaxByReward(this List<Tile> tiles)
        {
            if (!tiles.Any(x => x.Reward < 0))
                return tiles.MinBy(x => x.Reward);

            return tiles
                .Where(x => x.Reward < 0).MinBy(v => v.Reward);
        }
    }
}
