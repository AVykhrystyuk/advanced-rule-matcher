namespace AdvancedRuleMatcher
{
    public interface ISearchEngine
    {
        BaseRule? MatchRule(RuleMatchCriteria criteria);
    }

    public record RuleMatchCriteria(string Filter1, string Filter2, string Filter3, string Filter4);

    public record BaseRule(int RuleId, int? OutputValue);
}
