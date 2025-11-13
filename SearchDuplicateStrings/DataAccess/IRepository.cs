namespace SearchDuplicateStrings.DataAccess;

public interface IRepository
{
    string ReadRepository(string filePath);
    string[] ReadArrayFromRepository(string filePath);
    void WriteToRepository(string filePath, string fileContents, FileOutputFormat fileOutputFormat);
    void WriteDictionaryToRepository(string filePath, Dictionary<string, int> ddStrings, FileOutputFormat fileOutputFormat);
}

