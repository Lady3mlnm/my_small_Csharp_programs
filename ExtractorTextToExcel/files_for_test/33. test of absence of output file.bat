:: This command should cause an error since the output file doesn't exist.

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Test_French_numbers.txt ^
	--stringRange=10:16 ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Not_existing_output_file.xlsx ^
	--sheetOutput="Storage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1