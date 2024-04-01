using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GrammarRule {
    public string beforeString;
    public string afterString;
}


public class Grammar : MonoBehaviour {

    public enum SymbolType {
        Character,
        Word
    }
    public SymbolType symbolType = SymbolType.Word;


    public string startString = "A";

    public bool expandInParallel = false; // Whether or not we expand every rule in one go or not (i.e. is this grammar an L-System?). 


    public GrammarRule[] expansionRules;

    private string _currentString;
    public string currentString {
        get { return _currentString; }
    }

    void Awake() {
        _currentString = startString;
    }

    public void expandGrammar() {

        // Start by splitting the current string depending on our symbol type. 
        string[] stringSymbols = null;
        if (symbolType == SymbolType.Word) {
            stringSymbols = _currentString.Split(' '); // Split into words.
        }
        else if (symbolType == SymbolType.Character) {
            stringSymbols = new string[_currentString.Length];
            for (int c = 0; c < _currentString.Length; c++) {
                stringSymbols[c] = _currentString[c].ToString(); // Split into individual characters
            }
        }

        // Now apply our grammar rules to the array of symbols.
        applyRules(stringSymbols);

        // We now have a new string arranged in the stringSymbols array. 
        // All that remains is to rejoin all the parts. 
        if (symbolType == SymbolType.Word) {
            _currentString = string.Join(" ", stringSymbols);
        }
        else if (symbolType == SymbolType.Character) {
            _currentString = string.Join("", stringSymbols);
        }

    }


    public void applyRules(string[] symbols) {

        // YOUR CODE FOR TASK 1 HERE

        // Your goal is to look through the "symbols" array and find strings that match 
        // grammar rules in the "expansionRules" array. 
        // If a string matches a rule, then you should REPLACE it according to the rule.
        // (i.e. symbols[i] = rule.after)

        // Things to take into account: 

        // 1. If there are multiple rules that match a symbol, choose one RANDOMLY

        // 2. If "expandInParallel" is true, then you should expand EVERY symbol that matches a rule.
        // Otherwise, expand only the first symbol that matches a rule.

        // 3. When the symbols are entire words, then it's possible one of the symbols will contain
        // characters outside the symbol (i.e. "#location#." instead of "#location#").
        // You'll want to include these characters in the output, so you should use string.Contains and string.Replace as necessary.


    }

}
