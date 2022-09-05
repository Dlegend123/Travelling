using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Travelling
{
    public class Tile
    {
        public Tile((int, int) path, int reward, (int, int) distance, List<Tile> neighbours, Story story)
        {
            this.Path = path;
            this.Reward = reward;
            this.Distance = distance;
            this.Neighbours = neighbours;
            this.Story = story;
        }
        public Tile()
        {
            Neighbours = new List<Tile>();
            Distance = new ValueTuple<int, int>();
            Story = new();
        }
        public Tile(ValueTuple<int, int> distance, List<Tile> neighbours)
        {
            this.Distance = distance;
            this.Neighbours = neighbours;
            Story = new();
        }

        public Tile(ValueTuple<int, int> path, int reward)
        {
            this.Path = path;
            this.Reward = reward;
            Neighbours = new List<Tile>();
            Distance = new ValueTuple<int, int>();
            Story = new();
        }

        public (int, int) Path { get; set; }

        public int Reward { get; set; }

        public (int, int) Distance { get; set; }

        public List<Tile> Neighbours { get; set; }

        public Story Story { get; set; }
    }
}
