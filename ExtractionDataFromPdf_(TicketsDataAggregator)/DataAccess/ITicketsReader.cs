namespace TicketsDataAggregator.DataAccess;

public interface ITicketsReader
{
    string[] GetArrayDocumentsInStorage(string folderPath);

    string ReadDocument(string documentPath);
}