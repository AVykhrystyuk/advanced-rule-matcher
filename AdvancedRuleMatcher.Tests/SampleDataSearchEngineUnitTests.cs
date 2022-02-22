using AdvancedRuleMatcher.Impl;
using AdvancedRuleMatcher.Tests.Common;
using System.Collections.Generic;
using Xunit;

namespace AdvancedRuleMatcher.Tests
{
    public class SampleDataSearchEngineUnitTests : IClassFixture<SampleDataSearchEngineUnitTests.EngineFixture>
    {
        private readonly ISearchEngine engine;

        public SampleDataSearchEngineUnitTests(EngineFixture fixture)
            => engine = fixture.Engine;

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void TestFindRule(FourFilterRuleMatchCriteria criteria, int expectedRuleId)
        {
            var rule = engine.MatchRule(criteria);

            Assert.Equal(expectedRuleId, rule?.RuleId);
        }

        public static IEnumerable<object[]> GetTestCases() => SampleDataMetadata.GetTestCases();

        /// <summary>
        /// Shared object instance across tests in a single class
        /// </summary>
        public class EngineFixture
        {
            public ISearchEngine Engine { get; } = SearchEngine.Make(SampleDataMetadata.GetRules());
        }
    }
}
