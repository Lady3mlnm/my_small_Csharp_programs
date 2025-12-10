using ClosedXML.Excel;
using ExtractorExcelToText.DataStructures;
using System.Text;

namespace ExtractorExcelToText.DataAccess;

public interface IRepository
{
    XLWorkbook GetXLWorkbook(string pathInputExcel);
    IOrderedEnumerable<Record> ReadExcelColumn(ref XLWorkbook workbook, string sheetName, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark);
    IOrderedEnumerable<Record> ReadExcelTwoColumnsCombined(ref XLWorkbook workbook, string sheetName, string columnPositions, string columnTexts, string columnOverlay, string rowRange, string? cellIgnoringMark);
    string[] ReadTxt(string pathInputText, Encoding encoding);
    void WriteArrayToRepository(string filePath, string[] stringsReady, Encoding encoding);
}
