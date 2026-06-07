:: This command extracts names of amino acids and places them according to positions specified in Excel file.
:: The result is overlaid on top of the file content.
:: Some cells in the Excel column must be ignored.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=H ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeOverlay ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default