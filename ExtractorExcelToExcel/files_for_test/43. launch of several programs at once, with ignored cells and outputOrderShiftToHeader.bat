:: This command tests launch of several copies of the app at once. All copies work with the same input and output files.
:: This test is for check of work with ignored lines (parameter "cellIgnoringMark") and
::     under mode outputOrderMode.outputOrderShiftToHeader.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start /wait ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=2: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Amino Acids" ^
	--columnTextsOutput=A ^
	--headerDepthOutput=1 ^
	--outputOrderMode=outputOrderShiftToHeader ^
	--closeAppAfterExecution

start /wait ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=2: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=10 ^
	--outputOrderMode=outputOrderShiftToHeader ^
	--closeAppAfterExecution

start /wait ExtractorExcelToExcel.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=2: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestSheet" ^
	--columnTextsOutput=C ^
	--headerDepthOutput=20 ^
	--outputOrderMode=outputOrderShiftToHeader ^
	--closeAppAfterExecution