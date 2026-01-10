using ClosedXML.Excel;
using ExtractorExcelToText.DataStructures;
using System.Text;

namespace ExtractorExcelToText.DataAccess;

public interface IRepository
{
    FileStream GetFileStreamFromStorage(string pathInputExcel);
    IOrderedEnumerable<Record> ReadExcelColumn(FileStream fileStream, string sheetName, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark);
    IOrderedEnumerable<Record> ReadExcelTwoColumnsCombined(FileStream fileStream, string sheetName, string columnPositions, string columnTexts, string columnOverlay, string rowRange, string? cellIgnoringMark);
    string[] ReadTxt(string pathInputText, Encoding encoding);
    void WriteArrayToRepository(string filePath, string[] stringsReady, bool addEmptyLineToEnd, Encoding encoding);
}
