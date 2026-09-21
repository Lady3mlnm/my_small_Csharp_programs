using UglyToad.PdfPig;

namespace TicketsDataAggregator.DataAccess;

public class TicketsFromPdfsReader : ITicketsReader
{
    public string[] GetDocumentsInStorage(string folderPath)
    {
        return Directory.GetFiles(folderPath, "*.pdf");
    }

    public string ReadDocument(string pdfPath)
    {
        using PdfDocument document = PdfDocument.Open(pdfPath);
        if (document.NumberOfPages != 1)
            throw new InvalidOperationException("The PDF must contain exactly one page.");

        var page = document.GetPage(1);

        return page.Text;
    }
}