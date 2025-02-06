using TA_API.Models;

namespace TA_API.Interfaces;
public interface ICommentsService
{
    Task<List<Comment>> GetComments();
    Task<CommentsResults> ParseComments(string pattern);
}