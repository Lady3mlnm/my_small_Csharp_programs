:: Тест наиболее комплексного, двойного слияния данных.
:: Эта команда накладывает содержимое одного столбца Экселя поверх другого.
:: Результат накладывается поверх имеющегося в файле содержимого.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe ^
	--appMode=combineTwoColumns ^
	--pathExcelInput=Data\Test_Input.xlsx ^
	--sheetInput="Amino Acids" ^
	--columnPositions=A ^
	--columnTextsInput=B ^
	--columnTextsOverlay=H ^
	--rowRangeInput=2:4,6,10:15 ^
	--cellIgnoringMark="" ^
	--writingMode=modeOverlay ^
	--pathTxtOutput=Data\Test_Output.txt ^
	--emptyLineAtEnd ^
	--encoding=default