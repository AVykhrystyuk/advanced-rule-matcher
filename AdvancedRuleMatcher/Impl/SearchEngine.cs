using System.Collections.Generic;
using AdvancedRuleMatcher.Impl.Generic;

namespace AdvancedRuleMatcher.Impl
{
    public class SearchEngine : ISearchEngine
    {
        private readonly PrefixTreeSearchEngine<FourFilterRule> engine;

        public SearchEngine(PrefixTreeSearchEngine<FourFilterRule> engine) =>
            this.engine = engine;

        public FourFilterRule? MatchRule(FourFilterRuleMatchCriteria criteria)
        {
            var searchFields = new object[] { criteria.Filter1, criteria.Filter2, criteria.Filter3, criteria.Filter4 };
            var node = engine.Match(searchFields);
            return node?.Payload;
        }

        public static SearchEngine Make(IReadOnlyList<FourFilterRule> rules)
        {
            var anyKey = FourFilterRule.AnyFilter;

            var lookups = new LookupInfo<FourFilterRule>[]
            {
                new (r => r.Filter1, anyKey),
                new (r => r.Filter2, anyKey),
                new (r => r.Filter3, anyKey),
                new (r => r.Filter4, anyKey, PayloadSelector: GetHighestPriorityRule),
            };

            var engine = PrefixTreeSearchEngine<FourFilterRule>.Make(rules, lookups);
            return new SearchEngine(engine);
        }

        private static FourFilterRule? GetHighestPriorityRule(IReadOnlyList<FourFilterRule> rules)
        {
            FourFilterRule? prioritizedRule = null;

            foreach (var rule in rules)
            {
                if (prioritizedRule == null || rule.Priority > prioritizedRule.Priority)
                    prioritizedRule = rule; 
            }

            return prioritizedRule;
        }
    }
}
