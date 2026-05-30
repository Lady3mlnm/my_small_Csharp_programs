:: This command should cause an error since the output file doesn't contains the specified worksheet.

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=C ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Not existing output worksheet" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1