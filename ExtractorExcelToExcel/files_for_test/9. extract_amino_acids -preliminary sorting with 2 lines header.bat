:: This command extracts names of amino acids and places them according to positions specified in the input Excel file.
:: This is a check of preliminary sorting (parameter preliminarySortSheetByColumnPositions)
::     with 2 lines header (parameter headerDepthInput).

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="2 Rows Header" ^
	--columnPositions=A ^
	--columnTextsInput=C ^
	--preliminarySortSheetByColumnPositions=true ^
	--headerDepthInput=2 ^
	--rowRangeInput=4:10 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1