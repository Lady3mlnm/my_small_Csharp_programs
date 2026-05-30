:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: Rows from the header to the line 5 ("rowRangeInput=:5").
:: This command tests absence of the cell ignoring mark ("cellIgnoringMark=doNotUseCellIgnoring") and
:: output beginning from the first line of the worksheet ("headerDepthOutput=0").

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=C ^
	--rowRangeInput=:5 ^
	--cellIgnoringMark=doNotUseCellIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=0