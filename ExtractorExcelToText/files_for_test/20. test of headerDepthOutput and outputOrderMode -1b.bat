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
	--pathTxtOutput="Data\Test_Output -1b.txt" ^
	--headerDepthOutput=-1 ^
	--outputOrderMode=outputOrderAccordingToPositions ^
	--emptyLineAtEnd=false ^
	--encoding=default