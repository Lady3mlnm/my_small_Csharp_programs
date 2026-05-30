:: This command should cause an error since the input file doesn't exist.

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Not_existing_input_file.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=C ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1