:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: Rows from positions 10 ("rowRangeInput=10:").
:: This command tests absence of the cell ignoring mark ("cellIgnoringMark=doNotUseCellIgnoring") and
:: output beginning from the line 4 ("headerDepthOutput=3").

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=C ^
	--rowRangeInput=10: ^
	--cellIgnoringMark=doNotUseCellIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=3