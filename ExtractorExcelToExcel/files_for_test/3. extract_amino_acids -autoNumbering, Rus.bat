:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: This is a test of works with Cyrillic symbols.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Input.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTexts=D ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathOutputExcel=Data\Test_Output.xlsx ^
	--sheetNameOutput="TestStorage" ^
	--columnTextsOutput=C ^
	--headerDepth=1