using Microsoft.AspNetCore.Mvc;
using TA_API.Filters;

namespace TA_API.Controllers;

[ApiController]
[ServiceFilter<ErrorHandlingFilter>]
public class BaseApiController : ControllerBase
{

}
