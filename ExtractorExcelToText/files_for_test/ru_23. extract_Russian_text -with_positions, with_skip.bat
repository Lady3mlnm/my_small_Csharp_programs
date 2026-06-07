:: Эта команда извлекает русский текст и размещает их в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Dump PS2" ^
	--columnPositions=B ^
	--columnTextsInput=G ^
	--rowRangeInput=3:30 ^
	--cellIgnoringMark=. ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default