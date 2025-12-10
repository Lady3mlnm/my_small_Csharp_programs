:: Эта команда извлекает русский текст и размещает их в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.

start ExtractorExcelToText.exe extractOneColumn Data\Test_Excel.xlsx "Dump PS2" B G "3:30" . modeCreateNew Data\Test_Output.txt default