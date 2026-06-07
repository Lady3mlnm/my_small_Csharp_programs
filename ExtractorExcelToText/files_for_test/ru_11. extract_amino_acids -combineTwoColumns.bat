:: Эта команда накладывает содержимое одного столбца Экселя поверх другого.
:: Результат записывается в текстовый файл в соответствии с указанными в Excel позициями.

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default