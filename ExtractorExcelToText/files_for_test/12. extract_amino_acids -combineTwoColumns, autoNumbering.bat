:: This command overlays the contents of one Excel column on top of the other.
:: The result is written to a text file, strings in the same order as in Excel (auto-numbering is used).

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