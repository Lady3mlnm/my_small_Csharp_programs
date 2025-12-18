:: Эта команда извлекает русский текст и размещает их в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Dump PS2" ^
	--columnPositions=B ^
	--columnTexts=G ^
	--rowRange=3:30 ^
	--cellIgnoringMark=. ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default