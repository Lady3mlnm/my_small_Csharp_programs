start TextToHexConverter Data\Test_Input.txt Data\Test_Output.txt " " 2 true

:: parameter_1: file with original text
:: parameter_2: name of file to place result of the transformation
:: parameter_3: separator between hex-values
:: parameter_4: number of line break characters inserted in places of the line break in the original text. E.g., to add an empty line, place 2 of them.
:: parameter_5: whether to exclude symbols '\r\n' (transformed to 0D 0A) from the output. [possible valuses: true/false]

:: I recommend to use folder 'Data' for original texts and outputs but these are not required.