namespace TextToHexConverter.DataAccess;

internal class DiskRepository : IRepository
{
    public string ReadRepository(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public void WriteToRepository(string filePath, string fileContents)
    {
        File.WriteAllText(filePath, fileContents);
    }
}
