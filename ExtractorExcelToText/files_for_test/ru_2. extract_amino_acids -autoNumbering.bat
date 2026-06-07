:: Эта команда извлекает названия аминокислот и записывает их в текстовый файл в том же порядке, как в Экселе (используется автонумерация).

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=C ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default