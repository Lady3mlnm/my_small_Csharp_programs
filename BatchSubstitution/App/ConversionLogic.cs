using BatchSubstitution.DataAccess;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;


namespace BatchSubstitution.App;

public class ConversionLogic
{
    public Dictionary<string, string> StringsToDictionary(string[] lsDictionaryContent, string separatorsKeyValue, DictionaryDirectionFormat directionInDictionary)
    {
        Dictionary<string, string> ddReplacement = new();

        Action<string[]> addRecordToDictionary = directionInDictionary switch {
            DictionaryDirectionFormat.direct => (tp) => ddReplacement.Add(tp[0], tp[1]),
            DictionaryDirectionFormat.reverse => (tp) => ddReplacement.Add(tp[1], tp[0]),
            _ => throw new NotSupportedException(
                     "Unsupported value for parameter directionInDictionary in the method StringsToDictionary")
        };

        string[] keyValuePair;
        foreach(string st in lsDictionaryContent) {
            keyValuePair = st.Split(separatorsKeyValue);
            if(keyValuePair.Count() == 2)
                try {
                    addRecordToDictionary(keyValuePair);
                } catch(ArgumentException ex) {
                    throw new ArgumentException(
                        "Error during creation of dictionary for replacement. " +
                        $"Possible reason: string '{st}' in the dictionary file contains key that was already used earlier.", ex);
                }
            else if(keyValuePair.Count() > 2)
                throw new ArgumentException(
                    $"String '{st}' in the dictionary contains more than 1 separator of key and value. " +
                    "The application stops working to prevent faulty output.");
            // if keyValuePair.Count() == 1 then the app ignore this string
        }

        return ddReplacement;
    }


    public string BatchSubstitutionInText(string inputContent, Dictionary<string, string> ddReplacement, bool isOnlyWords=false)
    {
        string pattern = isOnlyWords
            ? @"\b(" + string.Join("|", ddReplacement.Keys) + @")\b"
            :    "(" + string.Join("|", ddReplacement.Keys) + ")";

        return Regex.Replace(inputContent, pattern, m => ddReplacement[m.Value]);
    }
}


