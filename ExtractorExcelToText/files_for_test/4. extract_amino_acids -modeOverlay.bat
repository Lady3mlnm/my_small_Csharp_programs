:: Эта команда извлекает названия аминокислот и размещает их в соответствии с указанными в Экселе позициями.
:: Результат накладывается поверх имеющегося в файле содержимого.

copy Data\Test_base.txt Data\Test_Output.txt

start ExtractorExcelToText.exe extractOneColumn Data\Test_Excel.xlsx "Amino Acids" A C "2:24" "" modeOverlay Data\Test_Output.txt true default