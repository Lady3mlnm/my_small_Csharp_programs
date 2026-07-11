using ExtractorExcelToText.App;
using ExtractorExcelToText.DataStructures;
using System.Text;

namespace ExtractorExcelToText.DataAccess;

public interface IRepository
{
    (Record[], int nmbLinesIgnoredAtStart) ReadRecordsFromRepository(string pathExcelInput, AppMode appMode, string sheetInput,
        string columnPositions, string columnTextsInput, string columnTextsOverlay,
        bool preliminarySortSheetByColumnPositions, int headerDepthInput, string rowRangeInput, string[] cellIgnoringMarks);
    string[] ReadTxt(string pathTxtInput, Encoding encoding);
    void WriteArrayToRepository(string pathTxtOutput, string[] stringsReady, bool addEmptyLineToEnd, Encoding encoding);
}
