using SparksApi.Analyzer.Champion;
using SparksApi.Analyzer.Item;
using SparksApi.Analyzer.Rune;
using SparksApi.Api.Handlers.Account;
using SparksApi.Api.Handlers.Item;
using SparksApi.Api.Handlers.Match;
using SparksApi.Api.Handlers.Runes;
using SparksApi.Api.Handlers.Summoner;
using SparksApi.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b => b
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(host => true)   // set to actual host/domain
            .AllowAnyHeader());
});

builder.Services.AddSingleton<ItemApiClient>();
builder.Services.AddSingleton<MatchApiClient>();
builder.Services.AddSingleton<RunesApiClient>();
builder.Services.AddSingleton<SummonerApiClient>();
builder.Services.AddSingleton<AccountApiClient>();
builder.Services.AddSingleton<ItemAnalyzer>();
builder.Services.AddSingleton<ChampionAnalyzer>();
builder.Services.AddSingleton<RuneAnalyzer>();
builder.Services.AddSingleton<ItemAnalyzerController>();
builder.Services.AddSingleton<MatchController>();
builder.Services.AddSingleton<ChampionAnalyzerController>();
builder.Services.AddSingleton<AccountController>();
builder.Services.AddSingleton<SummonerController>();
builder.Services.AddSingleton<RuneAnalyzerController>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();
