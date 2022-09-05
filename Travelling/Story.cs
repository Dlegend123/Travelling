namespace Travelling;

public class Story
{
    public Story(string title, List<string> paragraphs, string icon, List<KeyValuePair<int, string>> options,
        List<Tile> path)
    {
        this.Title = title;
        this.Paragraphs = paragraphs;
        this.Icon = icon;
        this.Options = options;
        this.Path = path;
    }

    public Story()
    {
        Title = "";
        Paragraphs = new List<string>();
        Icon = "";
        Options = new List<KeyValuePair<int, string>>();
        Path = new List<Tile>();
    }

    public void NextStage(ref Tile currentState)
    {
        var choice = -1;

        while (choice < 0 || choice > Path.Count - 1)
        {
            foreach (var t in Paragraphs)
            {
                Console.WriteLine(t);
                Console.Read();
            }

            foreach (var t in Options)
                Console.WriteLine(t.Key + " - " + t.Value);

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

        currentState = Path[choice];
    }

    public string Title { get; set; }

    public List<string> Paragraphs { get; set; }

    public string Icon { get; set; }

    public List<KeyValuePair<int, string>> Options { get; set; }

    public List<Tile> Path { get; set; }
}