namespace Travelling;

public class Story
{
    public Story(string title, List<string> paragraphs, string icon, List<KeyValuePair<int, string>> options,
        List<Tile> path)
    {
        Title = title;
        Paragraphs = paragraphs;
        Icon = icon;
        Options = options;
        StageList = path;
    }

    public Story()
    {
        Title = "";
        Paragraphs = new List<string>();
        Icon = "";
        Options = new List<KeyValuePair<int, string>>();
        StageList = new List<Tile>();
    }

    public string Title { get; set; }

    public List<string> Paragraphs { get; set; }

    public string Icon { get; set; }

    public List<KeyValuePair<int, string>> Options { get; set; }

    public List<Tile> StageList { get; set; }

    public Tile NextStage(int currentScore)
    {
        var choice = -1;

        while (choice < 0 || choice > StageList.Count - 1)
        {
            Paragraphs.ForEach(t =>
            {
                Console.WriteLine(t);
                Console.Read();
            });
            Options.ForEach(t => Console.WriteLine(t.Key + " - " + t.Value));

            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                choice = -1;
            }

            Console.Clear();
        }

        StageList[choice].Reward += currentScore;
        return StageList[choice];
    }
}