:: This command extracts from the text file several strings and places them to the Excel file.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Test_French_numbers.txt ^
	--stringRange=20: ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Storage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1