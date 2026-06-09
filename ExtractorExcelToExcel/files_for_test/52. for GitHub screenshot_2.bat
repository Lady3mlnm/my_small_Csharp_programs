:: This command overlays the contents of one Excel column on top of the other.
:: The result is written to a target file according to the positions specified in Excel.

copy Data\Test_base.xlsx Data\Test_Output.xlsx

start ExtractorExcelToExcel.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--cellIgnoringMark2="." ^
	--cellIgnoringMark3="*" ^
	--pathExcelOutput=Data\Test_Output.xlsx ^
	--sheetOutput=copyInputSheet ^
	--columnTextsOutput=copyInputColumn