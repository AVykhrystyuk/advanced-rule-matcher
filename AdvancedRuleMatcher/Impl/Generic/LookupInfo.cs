using System;
using System.Collections.Generic;

namespace AdvancedRuleMatcher.Impl.Generic
{
    public record LookupInfo<T>(Func<T, object> KeySelector, object AnyKey, Func<IReadOnlyList<T>, T?>? PayloadSelector = null);
}
