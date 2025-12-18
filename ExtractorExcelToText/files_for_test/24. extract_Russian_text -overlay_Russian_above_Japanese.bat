:: Эта команда извлекает японский и русский текты, русский накладывается поверх японского, результат размещается в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Dump PS2" ^
	--columnPositions=B ^
	--columnTexts=E ^
	--columnTextsOverlay=G ^
	--rowRange=3:30 ^
	--cellIgnoringMark=. ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default