using AdvancedRuleMatcher.Impl;
using System.IO;

namespace AdvancedRuleMatcher
{
    public static class SearchEngineFactory
    {
        public static ISearchEngine Create(FileInfo dataFile)
        {
            if (!dataFile.Exists)
                throw new FileNotFoundException("Data file is not found", fileName: dataFile.FullName);

            var fileReader = new CsvFourFilterRuleFileReader();
            var rules = fileReader.ReadAllRules(dataFile);

            return SearchEngine.Make(rules);
        }
    }
}
