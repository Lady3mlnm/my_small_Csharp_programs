:: This command extracts names of amino acids and places them according to positions specified in the input Excel file.
:: This is a check that the cells with empty lines (the value of the parameter 'cellIgnoringMark') are not carried to the target file.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Input.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=H ^
	--rowRange=2:21 ^
	--cellIgnoringMark="" ^
	--pathOutputExcel=Data\Test_Output.xlsx ^
	--sheetNameOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepth=1