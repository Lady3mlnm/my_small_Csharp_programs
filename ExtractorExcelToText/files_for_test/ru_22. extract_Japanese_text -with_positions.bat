:: Эта команда извлекает японский текст и размещает их в соответствии с указанными в Экселе позициями.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Dump PS2" ^
	--columnPositions=B ^
	--columnTextsInput=E ^
	--rowRangeInput=3:30 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default