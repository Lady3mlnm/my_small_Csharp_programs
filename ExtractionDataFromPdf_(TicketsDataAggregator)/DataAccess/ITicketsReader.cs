namespace TicketsDataAggregator.DataAccess;

public interface ITicketsReader
{
    string[] GetDocumentsInStorage(string folderPath);

    string ReadDocument(string documentPath);
}