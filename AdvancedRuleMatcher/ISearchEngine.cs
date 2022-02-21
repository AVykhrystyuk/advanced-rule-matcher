namespace AdvancedRuleMatcher
{
    public interface ISearchEngine
    {
        FourFilterRule? MatchRule(FourFilterRuleMatchCriteria criteria);
    }

    public record FourFilterRuleMatchCriteria(string Filter1, string Filter2, string Filter3, string Filter4);
}
