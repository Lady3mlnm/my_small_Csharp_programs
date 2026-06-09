:: This command should cause an error since the output file doesn't contains the specified worksheet.

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Test_French_numbers.txt ^
	--stringRange=10:16 ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Not existing output worksheet" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1