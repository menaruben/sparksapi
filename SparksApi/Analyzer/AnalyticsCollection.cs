using Sparks.Analyzer;
using System.Collections;

namespace SparksApi.Analyzer;

public sealed class AnalyticsCollection<T> : IEnumerable<T> where T : Analytic
{
    private readonly T[] _analytics;

    private AnalyticsCollection() => _analytics = Array.Empty<T>();
    private AnalyticsCollection(IEnumerable<T> analytics) => _analytics = analytics.ToArray();
    
    public static AnalyticsCollection<T> Empty() => new();
    public static AnalyticsCollection<T> From(IEnumerable<T> analytics) => new(analytics);
    
    public AnalyticsCollection<T> Add(T analytic) => new(_analytics.Append(analytic));
    public AnalyticsCollection<T> AddRange(IEnumerable<T> analytics) => new(_analytics.Concat(analytics));

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_analytics).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
