:: This command extracts names of amino acids and places them according to positions specified in Excel file.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="MultiIgnoring" ^
	--columnPositions=autoNumbering ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--rowRangeInput=: ^
	--cellIgnoringMark="" ^
	--cellIgnoringMark2="." ^
	--cellIgnoringMark3="_" ^
	--writingMode=modeOverlay ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default