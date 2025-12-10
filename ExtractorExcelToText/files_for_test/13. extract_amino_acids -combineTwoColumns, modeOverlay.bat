:: Эта команда накладывает содержимое одного столца Экселя поверх другого и размещает их в соответствии с указанными позициями.
:: Результат накладывается поверх имеющегося в файле содержимого.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe combineTwoColumns Data\Test_Excel.xlsx "Amino Acids" A B H 2:24 "" modeOverlay Data\Test_Output.txt default