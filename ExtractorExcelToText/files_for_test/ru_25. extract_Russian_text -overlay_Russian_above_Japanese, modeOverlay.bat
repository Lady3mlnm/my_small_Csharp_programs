:: Эта команда извлекает японский и русский текты, русский накладывается поверх японского, результат размещается в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.
:: Результат накладывается поверх имеющегося в файле содержимого.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Dump PS2" ^
	--columnPositions=B ^
	--columnTextsInput=E ^
	--columnTextsOverlay=G ^
	--rowRangeInput=3:30 ^
	--cellIgnoringMark=. ^
	--writingMode=modeOverlay ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default