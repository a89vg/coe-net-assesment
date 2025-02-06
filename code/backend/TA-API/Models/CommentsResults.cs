namespace TA_API.Models;

public class CommentsResults
{
    public List<Hit> NameResults { get; set; }

    public List<Hit> BodyResults { get; set; }
}

public class Hit
{
    public string Text { get; set; }

    public int Count { get; set; }
}


