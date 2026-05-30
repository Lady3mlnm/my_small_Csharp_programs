:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: This is a test of works with Cyrillic symbols.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=D ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=C ^
	--headerDepthOutput=1