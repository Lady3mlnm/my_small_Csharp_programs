using System.Text;

namespace TextToHexConverter.App;

public class ConversionLogic
{
    public string Convert(string inputContent, string separatorsHexValues, int nmbInsertedLinebreaks, bool isODOAExcludedFromHex)
    {
        // Transform text to hex
        byte[] bytes = Encoding.ASCII.GetBytes(inputContent);
        string hexString = BitConverter.ToString(bytes).Replace("-", separatorsHexValues);

        // Insert additional lines if this is required
        if (nmbInsertedLinebreaks > 0) {
            string stringReplaceLinebreaks =
                $"0D{separatorsHexValues}0A" + string.Concat(Enumerable.Repeat(Environment.NewLine, nmbInsertedLinebreaks));
            hexString = hexString.Replace($"0D{separatorsHexValues}0A{separatorsHexValues}", stringReplaceLinebreaks);
        }

        // Remove 0D_0A from output if this is required
        if (isODOAExcludedFromHex) {
            hexString = hexString.Replace($"{separatorsHexValues}0D{separatorsHexValues}0A", "")
                                 .Replace($"0D{separatorsHexValues}0A", "");
        }

        return hexString;
    }
}


            //string stringReplaceLinebreaks = isODOAExcludedFromHex
            //    ? string.Concat(Enumerable.Repeat(Environment.NewLine, nmbInsertedLinebreaks))
            //    : $"0D{separatorsHexValues}0A" + string.Concat(Enumerable.Repeat(Environment.NewLine, nmbInsertedLinebreaks));