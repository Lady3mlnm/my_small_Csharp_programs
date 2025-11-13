start BatchSubstitution Data\Test_Input.txt Data\Test_Dictionary.tbl " = " false direct Data\Test_Output.txt

:: parameter_1: file with original text
:: parameter_2: file with dictionary
:: parameter_3: separator between keys and values in the dictionary
:: parameter_4: Whether only whole words should be replaced. Possible values: false/true. "False" options allows to change combinations of symbols inside other words, but there can be false positive.
:: parameter_5: Direction of replacement: whether keys are changed to values or values to keys (provided they are unique). Possible values: direct/reverse.
:: parameter_6: name of file to place result of the transformation

:: I recommend to use folder 'Data' for original texts and outputs but these are not required.