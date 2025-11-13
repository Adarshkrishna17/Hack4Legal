using Amazon;
using Amazon.BedrockRuntime;
using EvidenceAnalyzerAPI.Interface;
using EvidenceAnalyzerAPI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IAmazonBedrockRuntime>(sp =>
{
    var config = new AmazonBedrockRuntimeConfig
    {
        RegionEndpoint = RegionEndpoint.EUWest2 // adjust your region
    };
    return new AmazonBedrockRuntimeClient(config);
});
// Register your service
// Register base Claude image service (used by others)
builder.Services.AddScoped<ClaudeImageAnalyzer>();

// Register analyzers
builder.Services.AddScoped<IClaudeImageAnalyzer, ClaudeImageAnalyzer>();
builder.Services.AddScoped<IClaudeVideoAnalyzer, ClaudeVideoAnalyzer>();
builder.Services.AddScoped<IClaudeAudioAnalyzer, ClaudeAudioAnalyzer>();
builder.Services.AddScoped<IClaudePDFAnalyzer, ClaudePDFAnalyzer>();
builder.Services.AddScoped<IClaudeOfficeAnalyzer, ClaudeOfficeAnalyzer>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();

app.Run();
