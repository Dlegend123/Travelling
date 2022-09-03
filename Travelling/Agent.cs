using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling
{
    public class Agent
    {
        private Tuple<int, int> state;
        private int score;

        public Agent(Tuple<int, int> state, int score)
        {
            State = state;
            Score = score;
        }

        public Tuple<int, int> State { get => state; set => state = value; }
        public int Score { get => score; set => score = value; }
    }
}
