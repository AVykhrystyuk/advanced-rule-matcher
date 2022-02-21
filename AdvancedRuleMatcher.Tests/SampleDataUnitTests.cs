using AdvancedRuleMatcher.Impl;
using System.Collections.Generic;
using Xunit;

namespace AdvancedRuleMatcher.Tests
{
    public class SampleDataUnitTests : IClassFixture<SampleDataUnitTests.EngineFixture>
    {
        private readonly ISearchEngine engine;

        public SampleDataUnitTests(EngineFixture fixture)
        {
            engine = fixture.Engine;
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void TestFindRule(RuleMatchCriteria criteria, int expectedRuleId)
        {
            var rule = engine.MatchRule(criteria);

            Assert.Equal(expectedRuleId, rule?.RuleId);
        }

        public static IEnumerable<object[]> GetTestCases()
        {
            static object[] Case(RuleMatchCriteria criteria, int expectedRuleId) => new object[] { criteria, expectedRuleId };

            yield return Case(new RuleMatchCriteria("AAA", "BBB", "CCC", "AAA"), expectedRuleId: 4);
            yield return Case(new RuleMatchCriteria("AAA", "BBB", "CCC", "DDD"), expectedRuleId: 4);
            yield return Case(new RuleMatchCriteria("AAA", "AAA", "AAA", "AAA"), expectedRuleId: 2);
            yield return Case(new RuleMatchCriteria("BBB", "BBB", "BBB", "BBB"), expectedRuleId: 6);
            yield return Case(new RuleMatchCriteria("BBB", "CCC", "CCC", "CCC"), expectedRuleId: 3);
        }

        /// <summary>
        /// Shared object instance across tests in a single class
        /// </summary>
        public class EngineFixture
        {
            public EngineFixture()
            {
                var any = FourFilterRule.AnyFilter;

                var rules = new[]
                {
                    new FourFilterRule(RuleId: 1, Priority: 80, "AAA", any, "CCC", "DDD", OutputValue: 8),
                    new FourFilterRule(RuleId: 2, Priority: 10, any, any, "AAA", any, OutputValue: 1),
                    new FourFilterRule(RuleId: 3, Priority: 70, "BBB", any, "CCC", any, OutputValue: 7),
                    new FourFilterRule(RuleId: 4, Priority: 100, "AAA", "BBB", "CCC", any, OutputValue: 10),
                    new FourFilterRule(RuleId: 5, Priority: 50, "CCC", "AAA", any, "CCC", OutputValue: 5),
                    new FourFilterRule(RuleId: 6, Priority: 0, any, any, any, any, OutputValue: 0),
                };
                Engine = new ExtremelyInefficientSearchEngine(rules);
            }

            public ISearchEngine Engine { get; private set; }
        }
    }
}
