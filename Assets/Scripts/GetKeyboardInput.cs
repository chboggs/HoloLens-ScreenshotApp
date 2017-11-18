using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetKeyboardInput : MonoBehaviour
{
    bool waiting = false;
    public string overridetext = "Jim";
    
    string GetText()
    {
        return GetComponent<Text>().text;
    }

    void SetText(string t)
    {
        GetComponent<Text>().text = t;
    }

    public void GetInput()
    {
        if (overridetext.Length != 0)
        {
            Debug.Log("overriding text for testing");
            SetText(overridetext);
            return;
        }
        try
        {
            UnityEngine.TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(GetText(), TouchScreenKeyboardType.Default, false, true, false, false);
            StartCoroutine(Waiting(keyboard));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator Waiting(UnityEngine.TouchScreenKeyboard keyboard)
    {
        if (waiting)
        {
            yield break;
        }
        waiting = true;

        while (!keyboard.done)
        {
            yield return null;
        }
        keyboard = null;
        SetText(keyboard.text);
        waiting = false;
    }
}