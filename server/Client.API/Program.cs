using Grpc.API;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var grpcService = builder.Configuration.GetConnectionString("GrpcService");
if (grpcService != null)
{
    builder.Services.AddGrpcClient<Finder.FinderClient>(x =>
    {
        x.Address = new Uri(grpcService);
    });
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
