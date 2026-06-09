:: This command should cause an error since the parameter "stringRange" in the form that is not supported.

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Test_French_numbers.txt ^
	--stringRange=10,11 ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Storage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1