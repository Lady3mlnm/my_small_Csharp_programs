:: Эта команда извлекает названия аминокислот и записывает их в текстовый файл в том же порядке, как в Экселе (используется автонумерация).
:: Проверка, как программа работает с русским текстом и кодировкой UTF-8.
:: Примеры задания альтернативных кодировок: ASCII, Windows-1252 (или, просто, 1252), Latin1, iso-8859-1.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTexts=D ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=UTF-8