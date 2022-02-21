using System.Collections.Generic;
using System.Linq;

namespace AdvancedRuleMatcher.Impl
{
    public class ExtremelyInefficientSearchEngine : ISearchEngine
    {
        private readonly IReadOnlyList<FourFilterRule> rules;

        public ExtremelyInefficientSearchEngine(IReadOnlyList<FourFilterRule> rules) => 
            this.rules = rules;

        public FourFilterRule? MatchRule(FourFilterRuleMatchCriteria criteria)
        {
            var any = FourFilterRule.AnyFilter;

            var filteredRules = rules
                .Where(r => r.Filter1 == criteria.Filter1 || r.Filter1 == any)
                .Where(r => r.Filter2 == criteria.Filter2 || r.Filter2 == any)
                .Where(r => r.Filter3 == criteria.Filter3 || r.Filter3 == any)
                .Where(r => r.Filter4 == criteria.Filter4 || r.Filter4 == any);

            return filteredRules
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();
        }
    }
}
