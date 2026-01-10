:: Эта команда накладывает содержимое одного столбца Экселя поверх другого.
:: Результат записывается в текстовый файл в соответствии с указанными в Excel позициями.

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=B ^
	--columnTextsOverlay=H ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default