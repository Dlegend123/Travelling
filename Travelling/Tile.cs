using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Travelling
{
    [StructLayout(LayoutKind.Sequential)]
    public class Tile
    {
        private ValueTuple<int, int> path;
        private int reward;
        private ValueTuple<int, int> distance;
        private List<Tile> neighbours;
        public Tile()
        {
            neighbours = new List<Tile>();
            distance = new ValueTuple<int, int>();
        }
        public Tile(ValueTuple<int, int> distance, List<Tile> neighbours)
        {
            this.Distance = distance;
            this.neighbours = neighbours;
        }

        public Tile(ValueTuple<int, int> path, int reward)
        {
            this.path = path;
            this.reward = reward;
            neighbours = new List<Tile>();
            distance = new ValueTuple<int, int>();
        }

        public ValueTuple<int, int> Path { get => path; set => path = value; }
        public int Reward { get => reward; set => reward = value; }
        public List<Tile> Neighbours { get => neighbours; set => neighbours = value; }
        public ValueTuple<int, int> Distance { get => distance; set => distance = value; }
    }
}
