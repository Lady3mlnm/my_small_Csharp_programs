:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: This is a test of special options "copyInputSheet" and "copyInputColumn".

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=C ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput=copyInputSheet ^
	--columnTextsOutput=copyInputColumn ^
	--headerDepthOutput=1