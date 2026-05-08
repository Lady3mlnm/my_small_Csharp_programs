:: This command should cause an error since the input file doesn't exist.

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Not_existing_input_file.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=C ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--pathOutputExcel=Data\Test_Output.xlsx ^
	--sheetNameOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepth=1