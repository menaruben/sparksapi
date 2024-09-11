using SparksApi.Api.Handlers.Match;

namespace SparksApi.Analyzer;

public interface IAnalyzer<T> where T : BaseAnalytic
{
    T[] Analyze(MatchParticipation[] participations);
}
