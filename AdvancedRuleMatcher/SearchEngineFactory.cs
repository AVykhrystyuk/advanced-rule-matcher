using AdvancedRuleMatcher.Impl;
using System.IO;

namespace AdvancedRuleMatcher
{
    public static class SearchEngineFactory
    {
        public static ISearchEngine Create(string dataFilePath = "Assets/SampleData.csv")
        {
            var dataFile = new FileInfo(Path.GetFullPath(dataFilePath)); 
            if (!dataFile.Exists)
                throw new FileNotFoundException("Data file is not found", fileName: dataFile.FullName);

            var fileReader = new CsvFourFilterRuleFileReader();
            var rules = fileReader.ReadAllRules(dataFile);

            return new ExtremelyInefficientSearchEngine(rules);
        }
    }
}
