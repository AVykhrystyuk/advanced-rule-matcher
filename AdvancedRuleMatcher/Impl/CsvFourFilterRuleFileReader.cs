using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvancedRuleMatcher.Impl
{
    public class CsvFourFilterRuleFileReader : IFourFilterRuleFileReader
    {
        private readonly CsvFileParser<CsvFourFilterRule> csvRulesParser = new();

        public IReadOnlyList<FourFilterRule> ReadAllRules(FileInfo file) =>
            csvRulesParser
                .Parse(file)
                .Select(csvRule => new FourFilterRule(
                    int.Parse(csvRule.RuleId!),
                    int.Parse(csvRule.Priority!),
                    csvRule.Filter1!,
                    csvRule.Filter2!,
                    csvRule.Filter3!,
                    csvRule.Filter4!,
                    int.Parse(csvRule.OutputValue!))
                )
                .ToArray();

        private class CsvFourFilterRule
        {
            public string? RuleId { get; set; }
            public string? Priority { get; set; }
            public string? Filter1 { get; set; }
            public string? Filter2 { get; set; }
            public string? Filter3 { get; set; }
            public string? Filter4 { get; set; }
            public string? OutputValue { get; set; }
        }
    }
}
