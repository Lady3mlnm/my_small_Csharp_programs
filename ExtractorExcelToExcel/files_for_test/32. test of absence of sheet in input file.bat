:: This command should cause an error since the input file doesn't contains the specified worksheet.

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Input.xlsx ^
	--sheetName="Not existing input worksheet" ^
	--columnPositions=A ^
	--columnTexts=C ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathOutputExcel=Data\Test_Output.xlsx ^
	--sheetNameOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepth=1