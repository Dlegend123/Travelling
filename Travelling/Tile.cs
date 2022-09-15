using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace Travelling;

public class Tile
{
    public Tile()
    {
        Neighbours = new List<Tile>();
        Distance = new ValueTuple<int, int>();
        Story = new Story();
        Score = 0;
    }

    internal Tile GetRandom()
    {
        return Neighbours[new Random().Next(Neighbours.Count)];
    }

    public (int, int) Path { get; set; }

    public int Reward { get; set; }

    public (int, int) Distance { get; set; }

    public List<Tile> Neighbours { get; set; }

    public Story Story { get; set; }
    public int Score { get; set; }
    public bool IsPortal { get; set; }
    public Tile Parent { get; set; }
    public double Cost { get; set; }
    public bool IsTrap { get; set; }
    public double CostDistance => Cost + (1 * (Distance.Item1 + Distance.Item2) +
                                          (Math.Sqrt(2) - 2 * 1) * Math.Min(Distance.Item1, Distance.Item2));
}