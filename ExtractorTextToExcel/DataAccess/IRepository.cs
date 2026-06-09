using System.Text;

namespace ExtractorTextToExcel.DataAccess;

public interface IRepository
{
    string[] ReadAllStringsFromRepository(string pathTxtInput, string stringRange, Encoding encoding);

    void WriteStringsToRepository(string[] strings, string pathExcelOutput, string sheetOutput,
        string columnTextsOutput, int headerDepthOutput, string[] stringIgnoringMarks);
}