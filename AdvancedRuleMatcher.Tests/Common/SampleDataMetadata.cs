using System.Collections.Generic;
using System.IO;

namespace AdvancedRuleMatcher.Tests.Common
{
    public class SampleDataMetadata
    {
        public static readonly FileInfo DataFile = new(Path.GetFullPath("Assets/SampleData.csv"));

        public static IReadOnlyList<FourFilterRule> GetRules()
        {
            var any = FourFilterRule.AnyFilter;

            return new[]
            {
                new FourFilterRule(RuleId: 1, Priority: 80, "AAA", any, "CCC", "DDD", OutputValue: 8),
                new FourFilterRule(RuleId: 2, Priority: 10, any, any, "AAA", any, OutputValue: 1),
                new FourFilterRule(RuleId: 3, Priority: 70, "BBB", any, "CCC", any, OutputValue: 7),
                new FourFilterRule(RuleId: 4, Priority: 100, "AAA", "BBB", "CCC", any, OutputValue: 10),
                new FourFilterRule(RuleId: 5, Priority: 50, "CCC", "AAA", any, "CCC", OutputValue: 5),
                new FourFilterRule(RuleId: 6, Priority: 0, any, any, any, any, OutputValue: 0),
            };
        }

        public static IEnumerable<object[]> GetTestCases()
        {
            static object[] Case(FourFilterRuleMatchCriteria criteria, int expectedRuleId) => new object[] { criteria, expectedRuleId };

            // initially provided test-cases
            yield return Case(new FourFilterRuleMatchCriteria("AAA", "BBB", "CCC", "AAA"), expectedRuleId: 4);
            yield return Case(new FourFilterRuleMatchCriteria("AAA", "BBB", "CCC", "DDD"), expectedRuleId: 4);
            yield return Case(new FourFilterRuleMatchCriteria("AAA", "AAA", "AAA", "AAA"), expectedRuleId: 2);
            yield return Case(new FourFilterRuleMatchCriteria("BBB", "BBB", "BBB", "BBB"), expectedRuleId: 6);
            yield return Case(new FourFilterRuleMatchCriteria("BBB", "CCC", "CCC", "CCC"), expectedRuleId: 3);


            const string _any_ = "ANYTHING";
            yield return Case(new FourFilterRuleMatchCriteria(_any_, _any_, _any_, _any_), expectedRuleId: 6);
            yield return Case(new FourFilterRuleMatchCriteria("AAA", "BBB", "CCC", _any_), expectedRuleId: 4);
            yield return Case(new FourFilterRuleMatchCriteria("BBB", _any_, "CCC", _any_), expectedRuleId: 3);
            yield return Case(new FourFilterRuleMatchCriteria("AAA", "DDD", "AAA", _any_), expectedRuleId: 2);
            yield return Case(new FourFilterRuleMatchCriteria("AAA", _any_, "CCC", "DDD"), expectedRuleId: 1);
            yield return Case(new FourFilterRuleMatchCriteria("CCC", "AAA", _any_, "CCC"), expectedRuleId: 5);
        }
    }
}
