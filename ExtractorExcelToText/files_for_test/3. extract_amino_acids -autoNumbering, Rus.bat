:: Эта команда извлекает названия аминокислот и записывает их в текстовый файл в том же порядке, как в Экселе (используется автонумерация).
:: Проверка, как программа работает с русским текстом и кодировкой uft-8.

start ExtractorExcelToText.exe extractOneColumn Data\Test_Excel.xlsx "Amino Acids" autoNumbering D 2:24 "" modeCreateNew Data\Test_Output.txt true utf-8