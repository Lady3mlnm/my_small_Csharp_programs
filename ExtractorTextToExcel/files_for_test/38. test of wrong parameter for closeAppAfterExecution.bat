:: This command should cause an error since a given value for the closeAppAfterExecution parameter is not a boolean.

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Test_French_numbers.txt ^
	--stringRange=10:16 ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Storage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1 ^
        --closeAppAfterExecution=CloseTheApp