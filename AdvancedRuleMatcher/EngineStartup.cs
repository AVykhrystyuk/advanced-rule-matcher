using AdvancedRuleMatcher.Impl;
using System.IO;

namespace AdvancedRuleMatcher
{
    public static class EngineStartup
    {
        public static ISearchEngine Start(string dataFilePath = "Assets/SampleData.csv")
        {
            var dataFile = new FileInfo(Path.GetFullPath(dataFilePath)); 
            if (!dataFile.Exists)
                throw new FileNotFoundException("Data file is not found", fileName: dataFile.FullName);
            

            return new ExtremelyInefficientSearchEngine(null);
        }
    }
}
