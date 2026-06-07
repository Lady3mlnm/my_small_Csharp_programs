:: Эта команда извлекает японский текст и записывает его в текстовый файл в том же порядке, как в Экселе (используется автонумерация).

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Dump PS2" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=E ^
	--rowRangeInput=3:13 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd=false ^
	--encoding=default