using AdvancedRuleMatcher.Impl;
using System;
using System.Collections.Generic;
using Xunit;

namespace AdvancedRuleMatcher.Tests
{
    public class SearchEngineUnitTests
    {
        [Fact]
        public void TestFindRuleWithoutAnyFilters()
        {
            var engine = SearchEngine.Make(rules: Array.Empty<FourFilterRule>());

            var rule = engine.MatchRule(new FourFilterRuleMatchCriteria("A", "B", "C", "D"));

            Assert.Null(rule);
        }

        private static List<FourFilterRule> GetRulesWithExactFilters() => new()
        {
            // special "any" filter is not used here
            new FourFilterRule(RuleId: 1, Priority: 80, "A", "B", "C", "D"),
            new FourFilterRule(RuleId: 4, Priority: 95, "A", "A", "A", "A"),
            new FourFilterRule(RuleId: 5, Priority: 50, "B", "B", "B", "B"),
        };

        [Fact]
        public void TestFindRuleFindsNothingForExactFiltersIfNoExactMatch()
        {
            var engine = SearchEngine.Make(GetRulesWithExactFilters());

            var rule = engine.MatchRule(new FourFilterRuleMatchCriteria("X", "Y", "Z", "!"));

            Assert.Null(rule);
        }

        [Fact]
        public void TestFindRuleFindsNothingForNoMatchWithAnyFilters()
        {
            var any = FourFilterRule.AnyFilter;

            var rules = GetRulesWithExactFilters();
            rules.Add(new FourFilterRule(RuleId: 10, Priority: 100, any, any, any, "D"));

            var engine = SearchEngine.Make(rules);

            var rule = engine.MatchRule(new FourFilterRuleMatchCriteria("X", "Y", "Z", "!"));

            Assert.Null(rule);
        }

        [Fact]
        public void TestFindRuleFindsRuleForExactFiltersAndExactMatch()
        {
            var engine = SearchEngine.Make(GetRulesWithExactFilters());

            var rule = engine.MatchRule(new FourFilterRuleMatchCriteria("A", "B", "C", "D"));

            Assert.Equal(expected: 1, rule?.RuleId);
        }

        [Fact]
        public void TestFindRuleRespectsPriorityForExactFilters()
        {
            var engine = SearchEngine.Make(new[]
            {
                new FourFilterRule(RuleId: 1, Priority: 80, "A", "A", "A", "A"),
                new FourFilterRule(RuleId: 2, Priority: 95, "A", "A", "A", "A"),
                new FourFilterRule(RuleId: 3, Priority: 50, "A", "A", "A", "A"),
                new FourFilterRule(RuleId: 4, Priority: 100, "B", "B", "B", "B"),
            });

            var rule = engine.MatchRule(new FourFilterRuleMatchCriteria("A", "A", "A", "A"));

            Assert.Equal(expected: 2, rule?.RuleId);
        }

        [Fact]
        public void TestFindRuleRespectsPriorityIfThereAreAnyFilters()
        {
            var any = FourFilterRule.AnyFilter;

            var engine = SearchEngine.Make(new[]
            {
                new FourFilterRule(RuleId: 1, Priority: 80, "A", "A", "A", "A"),
                new FourFilterRule(RuleId: 2, Priority: 95, "A", "A", "A", any),
                new FourFilterRule(RuleId: 3, Priority: 0, any, any, any, any),
                new FourFilterRule(RuleId: 4, Priority: 50, "B", "B", "B", "B"),
            });

            var rule = engine.MatchRule(new FourFilterRuleMatchCriteria("A", "A", "A", "A"));

            Assert.Equal(expected: 2, rule?.RuleId);
        }
    }
}
