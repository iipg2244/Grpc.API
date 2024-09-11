namespace Client.API.Controllers;

using Grpc.API;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class FinderController : ControllerBase
{
    private readonly ILogger<FinderController> _logger;
    private readonly Finder.FinderClient _finderClient;

    public FinderController(ILogger<FinderController> logger, Finder.FinderClient finderClient)
    {
        _logger = logger;
        _finderClient = finderClient;
    }

    [HttpPost(Name = "search")]
    public QueryReply GetSearch([FromBody] QueryRequest queryRequest) => _finderClient.GetSearch(queryRequest);

}
