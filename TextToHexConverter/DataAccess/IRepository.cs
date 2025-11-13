namespace TextToHexConverter.DataAccess;

public interface IRepository
{
    string ReadRepository(string filePath);
    void WriteToRepository(string filePath, string fileContents);
}

