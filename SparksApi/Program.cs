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

builder.Services.AddScoped<ItemApiClient>();
builder.Services.AddScoped<MatchApiClient>();
builder.Services.AddScoped<RunesApiClient>();
builder.Services.AddScoped<SummonerApiClient>();
builder.Services.AddScoped<AccountApiClient>();
builder.Services.AddScoped<ItemAnalyzer>();
builder.Services.AddScoped<ChampionAnalyzer>();
builder.Services.AddScoped<RuneAnalyzer>();
builder.Services.AddScoped<ItemAnalyzerController>();
builder.Services.AddScoped<MatchController>();
builder.Services.AddScoped<ChampionAnalyzerController>();
builder.Services.AddScoped<AccountController>();
builder.Services.AddScoped<SummonerController>();
builder.Services.AddScoped<RuneAnalyzerController>();

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
