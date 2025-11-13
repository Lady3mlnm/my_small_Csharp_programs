namespace SearchDuplicateStrings.App;

public class TextProcessingLogic
{
    public Dictionary<string, int> FindDuplicates(string[] arStrings)
    {
        Dictionary<string, int> repeates = arStrings.GroupBy(st => st)
                                                    .Where(g => g.Count() > 1)
                                                    .ToDictionary(g => g.Key, g => g.Count());
        return repeates;

        // === alternative way of creating and filtering the dictionary === //
        //Dictionary<string, int> repeatedWords = arStrings.GroupBy(st => st)
        //                                                 .ToDictionary(g => g.Key, g => g.Count())  // create a dictionary, containing frequency of words
        //                                                 .Where(pair => pair.Value > 1)             // filter the dictionary
        //                                                 .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
