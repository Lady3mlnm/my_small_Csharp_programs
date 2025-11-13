namespace BatchSubstitution.DataAccess;

public interface IRepository
{
    string ReadRepositoryText(string filePath);
    string[] ReadRepositoryListStrings(string filePath);
    void WriteToRepository(string filePath, string fileContents);
}