using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordAssigner : MonoBehaviour {
    public string CustomKeyWord = "";
    public bool UseKeyWord = true;
    public string GetKeyWord()
    {
        if (CustomKeyWord.Length != 0)
        {
            return CustomKeyWord;
        }
        return GetComponentInChildren<TextMesh>().text;
    }
}
