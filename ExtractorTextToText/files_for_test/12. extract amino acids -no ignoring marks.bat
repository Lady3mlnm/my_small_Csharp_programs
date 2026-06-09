:: This command extracts from the text file all strings and places them to the Excel file.
:: This is test of work of the parameter "cellIgnoringMark".

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorTextToExcel.exe ^
	--pathTxtInput=Data\Test_Input.txt ^
	--stringRange=: ^
	--stringIgnoringMark=doNotUseStringIgnoring ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Storage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1