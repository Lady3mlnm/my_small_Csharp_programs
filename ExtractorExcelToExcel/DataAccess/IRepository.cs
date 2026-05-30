using ExtractorExcelToExcel.App;
using ExtractorExcelToExcel.DataStructures;

namespace ExtractorExcelToExcel.DataAccess;

public interface IRepository
{
    Record[] ReadRecordsFromRepository(string pathExcelInput, AppMode appMode, string sheetInput,
        string columnPositions, string columnTextsInput, string columnTextsOverlay,
        bool preliminarySortSheetByColumnPositions, int headerDepthInput, string rowRangeInput, string? cellIgnoringMark);

    void WriteRecordsToRepository(IEnumerable<Record> records, string pathOutputExcel,
        string sheetNameOutput, string columnTextsOutput, int headerDepth);
}