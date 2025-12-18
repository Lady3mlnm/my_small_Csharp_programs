:: Эта команда накладывает содержимое одного столца Экселя поверх другого и записывает их в текстовый файл в том же порядке, как в Экселе (используется автонумерация).

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTexts=B ^
	--columnTextsOverlay=H ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default