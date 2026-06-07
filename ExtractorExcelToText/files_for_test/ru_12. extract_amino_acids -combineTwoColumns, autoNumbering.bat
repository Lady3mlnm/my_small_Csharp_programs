:: Эта команда накладывает содержимое одного столбца Экселя поверх другого.
:: Результат записывается в текстовый файл, строки в том же порядке, как в Экселе (используется автонумерация).

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default