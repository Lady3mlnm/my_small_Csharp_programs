using ExtractorExcelToText.DataStructures;
using System.Text;

namespace ExtractorExcelToText.DataAccess;

public interface IRepository
{
    Record[] ReadExcelColumn(string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark);
    Record[] ReadExcelTwoColumnsCombined(string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string columnOverlay, string rowRange, string? cellIgnoringMark);
    string[] ReadTxt(string pathInputText, Encoding encoding);
    void WriteArrayToRepository(string filePath, string[] stringsReady, bool addEmptyLineToEnd, Encoding encoding);
}
