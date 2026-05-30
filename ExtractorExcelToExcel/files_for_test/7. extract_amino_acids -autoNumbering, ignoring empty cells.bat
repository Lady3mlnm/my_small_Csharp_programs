:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: This is a check that the cells with empty lines (the value of the parameter 'cellIgnoringMark') are not carried to the target file.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=H ^
	--rowRangeInput=2:21 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1