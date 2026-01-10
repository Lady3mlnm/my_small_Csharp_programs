:: This command extracts names of amino acids and writes them to text file in the same order as they are placed in Excel file (auto-numbering is used).
:: This is a test how the program works with Cyrillic symbols and endoding UTF-8.
:: Examples of alternative encodings: ASCII, Windows-1252 (or simply 1252), Latin1, iso-8859-1.

start ExtractorExcelToText.exe ^
	--appMode=extractOneColumn ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=autoNumbering ^
	--columnTexts=D ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeCreateNew ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=UTF-8