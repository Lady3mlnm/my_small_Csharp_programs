:: Тест наиболее комплексного, двойного слияния данных.
:: Эта команда накладывает содержимое одного столбца Экселя поверх другого.
:: Результат накладывается поверх имеющегося в файле содержимого.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathInputExcel=Data\Test_Excel.xlsx ^
	--sheetName="Amino Acids" ^
	--columnPositions=A ^
	--columnTexts=B ^
	--columnTextsOverlay=H ^
	--rowRange=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeOverlay ^
	--pathTxt=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default