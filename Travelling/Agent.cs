namespace Travelling;

public class Agent
{
    public Agent(Tuple<int, int> state, int score)
    {
        State = state;
        Score = score;
    }

    public Tuple<int, int> State { get; set; }

    public int Score { get; set; }
}