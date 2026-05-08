:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: This is a test of special options "copyInputSheet" and "copyInputColumn".

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Input.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTexts=C ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathOutputExcel=Data\Test_Output.xlsx ^
	--sheetNameOutput=copyInputSheet ^
	--columnTextsOutput=copyInputColumn ^
	--headerDepth=1