:: Эта команда извлекает названия аминокислот и размещает их в соответствии с указанными в Экселе позициями.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=C ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default