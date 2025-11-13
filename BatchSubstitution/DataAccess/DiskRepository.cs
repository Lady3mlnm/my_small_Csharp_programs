namespace BatchSubstitution.DataAccess;

internal class DiskRepository : IRepository
{
    public string ReadRepositoryText(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public string[] ReadRepositoryListStrings(string filePath)
    {
        string separator = Environment.NewLine;
        string fileContents = File.ReadAllText(filePath);
        return fileContents.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }

    public void WriteToRepository(string filePath, string fileContents)
    {
        File.WriteAllText(filePath, fileContents);
    }
}