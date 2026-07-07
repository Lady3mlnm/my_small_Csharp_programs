:: Test of the parameters 'headerDepthOutput' and 'outputOrderMode' - starting point.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=C ^
	--rowRangeInput=10:19 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput="Data\Test_Output -1a.txt" ^
	--headerDepthOutput=0 ^
	--outputOrderMode=outputOrderAccordingToPositions ^
	--emptyLineAtEnd=false ^
	--encoding=default