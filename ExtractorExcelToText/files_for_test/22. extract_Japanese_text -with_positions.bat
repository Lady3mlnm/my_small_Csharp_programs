:: Эта команда извлекает японский текст и размещает их в соответствии с указанными в Экселе позициями.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Dump PS2" ^
	--columnPositions=B ^
	--columnTexts=E ^
	--rowRange=3:30 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default