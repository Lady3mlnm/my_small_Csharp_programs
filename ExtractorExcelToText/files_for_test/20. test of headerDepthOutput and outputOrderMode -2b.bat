:: Test of the parameters 'headerDepthOutput' and 'outputOrderMode'.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=C ^
	--rowRangeInput=10:19 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput="Data\Test_Output -2b.txt" ^
	--headerDepthOutput=10 ^
	--outputOrderMode=outputOrderShiftToHeader ^
	--emptyLineAtEnd=false ^
	--encoding=default