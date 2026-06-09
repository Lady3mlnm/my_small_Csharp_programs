:: This command should cause an error since the input file doesn't exist.

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Not_existing_file.txt ^
	--stringRange=10:16 ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Storage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1