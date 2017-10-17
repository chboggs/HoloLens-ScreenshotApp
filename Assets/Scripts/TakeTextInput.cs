using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeTextInput : MonoBehaviour {
    UnityEngine.TouchScreenKeyboard keyboard;
    public static string keyboardText = "";


    public void TakeKeyboardInput()
    {
        try
        {
            keyboard = TouchScreenKeyboard.Open("starttexxt", TouchScreenKeyboardType.Default, false, true, false, false);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            if (keyboard.done == true)
            {
                keyboardText = keyboard.text;
                keyboard = null;
                Debug.Log("you typed: '" + keyboardText + "'");
                GetComponentInChildren<TextMesh>().text = keyboardText;
            }
        }
    }


}
