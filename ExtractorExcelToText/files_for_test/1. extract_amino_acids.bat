:: This command extracts names of amino acids and places them according to positions specified in Excel file.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=C ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default