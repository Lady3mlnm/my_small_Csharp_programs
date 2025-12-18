:: Эта команда извлекает японский текст и записывает его в текстовый файл в том же порядке, как в Экселе (используется автонумерация).

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Dump PS2" ^
	--columnPositions=autoNumbering ^
	--columnTexts=E ^
	--rowRange=3:13 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd=false ^
	--encoding=default