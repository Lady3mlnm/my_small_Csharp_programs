namespace SearchFormulasInExcel.DataStructures;

public struct RecordWithFormula
{
    public int Position;
    public string FormulaBody;

    public RecordWithFormula(int position, string formulaBody)
    {
        Position = position;
        FormulaBody = formulaBody;
    }

    public override string ToString() =>
        $"{Position}. ={FormulaBody}";
}