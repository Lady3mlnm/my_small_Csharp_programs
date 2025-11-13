namespace SearchDuplicateStrings.DataAccess;

internal class DiskRepository : IRepository
{
    public string ReadRepository(string filePath)
    {
        return File.ReadAllText(filePath);
    }


    public string[] ReadArrayFromRepository(string filePath)
    {
        string separator = Environment.NewLine;
        string fileContents = File.ReadAllText(filePath);
        return fileContents.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }


    public void WriteToRepository(string filePath, string fileContents, FileOutputFormat fileOutputFormat)
    {
        File.WriteAllText(filePath, fileContents);
    }


    public void WriteDictionaryToRepository(string filePath, Dictionary<string, int> ddStrings, FileOutputFormat fileOutputFormat)
    {
        IEnumerable<string> fileFragments = fileOutputFormat switch {
            FileOutputFormat.asStrings => ddStrings
                .Select(pair => string.Join(Environment.NewLine, Enumerable.Repeat(pair.Key, pair.Value))),
            FileOutputFormat.asDictionary => ddStrings
                .Select(pair => $"{pair.Value} ← {pair.Key}"),
            _ => throw new ArgumentException(
                "Function 'WriteDictionaryToRepository' received unsupported format for output file: " + fileOutputFormat)
        };

        File.WriteAllText(filePath, string.Join(Environment.NewLine, fileFragments));
    }
}
