:: This command tests how the program works with several ignoring marks.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="MultiIgnoring" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=H ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--cellIgnoringMark2="." ^
	--cellIgnoringMark3="_" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1