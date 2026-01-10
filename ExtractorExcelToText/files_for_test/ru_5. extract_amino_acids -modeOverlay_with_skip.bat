:: Эта команда извлекает названия аминокислот и размещает их в соответствии с указанными в Экселе позициями.
:: Результат накладывается поверх имеющегося в файле содержимого.
:: Некоторые ячейки с столбце Экселя подлежат игнорированию.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=H ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeOverlay ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default