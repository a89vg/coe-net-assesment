using System.Text.RegularExpressions;
using TA_API.Interfaces;
using TA_API.Models;

namespace TA_API.Services;

public class CommentsService : ICommentsService
{
    private readonly HttpClient client;
    private const string CommentsUrl = "https://jsonplaceholder.typicode.com/comments";

    public CommentsService(IHttpClientFactory clientFactory)
    {
        client = clientFactory.CreateClient();
    }

    public async Task<List<Comment>> GetComments()
    {
        var response = await client.GetAsync(CommentsUrl);

        var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();

        return comments;
    }

    public async Task<CommentsResults> ParseComments(string pattern)
    {
        var response = await client.GetAsync(CommentsUrl);

        var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();
        
        var nameContents = comments.Select(c => c.Name);
        var bodyContents = comments.Select(c => c.Body);

        var nameResults = ExtractHits(pattern, nameContents);
        var bodyResults = ExtractHits(pattern, bodyContents);

        return new CommentsResults
        {
            NameResults = nameResults,
            BodyResults = bodyResults
        };
    }

    private static List<Hit> ExtractHits(string pattern, IEnumerable<string> contents)
    {
        var results = new Dictionary<string, int>();

        var regex = @$"\b{pattern}(\w+)?\b";

        foreach (var name in contents.Where(s => Regex.IsMatch(s, regex)))
        {
            var matches = Regex.Matches(name, regex);

            foreach (var hit in matches.Select(m => m.Groups[0].Value))
            {
                if (results.TryGetValue(hit, out var value))
                {
                    results[hit] = ++value;
                }
                else
                {
                    results.Add(hit, 1);
                }
            }
        }

        return results.Select(h => new Hit { Text = h.Key, Count = h.Value }).ToList();
    }
}
