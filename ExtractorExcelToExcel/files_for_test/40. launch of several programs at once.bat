:: This command test launch of several copies of the app at once. All copies work with the same input and output files.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start /wait ExtractorExcelToExcel.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--preliminarySortSheetByColumnPositions=true ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="Amino Acids" ^
	--columnTextsOutput=A ^
	--headerDepthOutput=1 ^
	--closeAppAfterExecution

start /wait ExtractorExcelToExcel.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--preliminarySortSheetByColumnPositions=true ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestStorage" ^
	--columnTextsOutput=B ^
	--headerDepthOutput=1 ^
	--closeAppAfterExecution

start /wait ExtractorExcelToExcel.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--preliminarySortSheetByColumnPositions=true ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput="TestSheet" ^
	--columnTextsOutput=C ^
	--headerDepthOutput=1 ^
	--closeAppAfterExecution