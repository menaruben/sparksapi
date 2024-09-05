using Sparks.Analyzer;
using SparksApi.Api.Handlers.Match;

namespace SparksApi.Analyzer;

public interface IAnalyzer<T> where T : Analytic
{
    AnalyticsCollection<T> Analyze(IEnumerable<MatchParticipation> participation);
}
