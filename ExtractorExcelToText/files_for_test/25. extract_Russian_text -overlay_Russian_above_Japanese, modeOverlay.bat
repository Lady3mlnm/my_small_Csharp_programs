:: Эта команда извлекает японский и русский текты, русский накладывается поверх японского, результат размещается в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.
:: Результат накладывается поверх имеющегося в файле содержимого.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Dump PS2" ^
	--columnPositions=B ^
	--columnTexts=E ^
	--columnTextsOverlay=G ^
	--rowRange=3:30 ^
	--cellIgnoringMark=. ^
	--writingMode=modeOverlay ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default