:: This command extracts names of amino acids and places them according to positions specified in the input Excel file.
:: This is a check that the cells with empty lines (the value of the parameter 'cellIgnoringMark') are not carried to the target file.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=2:21 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1