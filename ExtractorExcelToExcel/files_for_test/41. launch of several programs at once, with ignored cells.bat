:: This command tests launch of several copies of the app at once. All copies work with the same input and output files.
:: This test is for check of work with ignored lines (parameter "cellIgnoringMark").

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start /wait ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Amino Acids" ^
	--columnTextsOutput=A ^
	--headerDepthOutput=1 ^
	--closeAppAfterExecution

start /wait ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1 ^
	--closeAppAfterExecution

start /wait ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestSheet" ^
	--columnTextsOutput=C ^
	--headerDepthOutput=1 ^
	--closeAppAfterExecution