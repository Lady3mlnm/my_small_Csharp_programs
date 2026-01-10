:: This command extracts names of amino acids and places them according to positions specified in Excel file.
:: The result is overlaid on top of the file content.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=C ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeOverlay ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default