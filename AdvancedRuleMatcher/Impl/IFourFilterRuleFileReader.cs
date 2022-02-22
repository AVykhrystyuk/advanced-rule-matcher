using System.Collections.Generic;
using System.IO;

namespace AdvancedRuleMatcher.Impl
{
    public interface IFourFilterRuleFileReader
    {
        IReadOnlyList<FourFilterRule> ReadAllRules(FileInfo file);
    }
}