using System.Collections.Generic;
using Xunit;

namespace AdvancedRuleMatcher.Tests
{
    public class SampleDataIntegrationTests : IClassFixture<SampleDataIntegrationTests.EngineFixture>
    {
        private readonly ISearchEngine engine;

        public SampleDataIntegrationTests(EngineFixture fixture)
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
                Engine = EngineStartup.Start();
            }

            public ISearchEngine Engine { get; private set; }
        }
    }
}
