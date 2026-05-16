:: This command extracts names of amino acids and places them according to positions specified in the input Excel file.
:: This is a check of preliminary sorting (parameter preliminarySortSheetByColumnPositions).

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Input.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=C ^
	--preliminarySortSheetByColumnPositions=true ^
	--rowRange=4:10 ^
	--cellIgnoringMark="" ^
	--pathOutputExcel=Data\Test_Output.xlsx ^
	--sheetNameOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepth=1