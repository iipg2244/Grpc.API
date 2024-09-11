using Grpc.API;
using Grpc.Core;

namespace Grpc.API.Services;

public class FinderService : Finder.FinderBase
{
    private readonly ILogger<FinderService> _logger;

    public FinderService(ILogger<FinderService> logger) => _logger = logger;

    public override Task<QueryReply> GetSearch(QueryRequest request, ServerCallContext context) => Task.FromResult(new QueryReply
    {
        Result = $"The query '{request.Consultation}' has been sent to the indicated region '{request.Region}'."
    });
}
