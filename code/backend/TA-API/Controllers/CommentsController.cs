using Microsoft.AspNetCore.Mvc;
using TA_API.Interfaces;

namespace TA_API.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentsController : BaseApiController
{
    private readonly ICommentsService commentsService;

    public CommentsController(ICommentsService commentsService)
    {
        this.commentsService = commentsService;
    }

    [HttpGet]

    public async Task<IActionResult> GetComments()
    {
        var response = await commentsService.GetComments();

        return Ok(response);

    }

    [HttpGet("parsed")]

    public async Task<IActionResult> ParseComments([FromQuery] string pattern)
    {
        var response = await commentsService.ParseComments(pattern);

        return Ok(response);

    }
}
