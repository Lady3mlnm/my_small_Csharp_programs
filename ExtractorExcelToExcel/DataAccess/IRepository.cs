using ExtractorExcelToExcel.App;
using ExtractorExcelToExcel.DataStructures;

namespace ExtractorExcelToExcel.DataAccess;

public interface IRepository
{
    Record[] ReadRecordsFromRepository(string pathInputExcel, AppMode appMode,
        string sheetName, string columnPositions, string columnTexts,
        string columnTextsOverlay, string rowRange, string? cellIgnoringMark);

    void WriteRecordsToRepository(IEnumerable<Record> records, string pathOutputExcel,
        string sheetNameOutput, string columnTextsOutput, int headerDepth);
}