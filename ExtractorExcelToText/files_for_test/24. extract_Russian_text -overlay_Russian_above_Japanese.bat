:: Эта команда извлекает японский и русский текты, русский накладывается поверх японского, результат размещается в соответствии с указанными в Экселе позициями.
:: Ячейки, в которых располагается точка, игнорируются.

start ExtractorExcelToText.exe combineTwoColumns Data\Test_Excel.xlsx "Dump PS2" B E G "3:30" . modeCreateNew Data\Test_Output.txt true default