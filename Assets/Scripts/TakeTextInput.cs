using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TakeTextInput : MonoBehaviour {
    UnityEngine.TouchScreenKeyboard keyboard;
    public string KeyBoardText = "";

    public void TakeKeyboardInput()
    {
        try
        {
            keyboard = TouchScreenKeyboard.Open(KeyBoardText, TouchScreenKeyboardType.Default, false, true, false, false);
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
                KeyBoardText = keyboard.text;
                keyboard = null;
                Debug.Log("you typed: '" + KeyBoardText + "'");
                //GetComponentInChildren<Text>().text = keyboardText;
                if (KeyBoardText.Length != 0)
                {
                    GetComponentInChildren<TextMesh>().text = "Edit\nCaption";
                }
                else
                {
                    GetComponentInChildren<TextMesh>().text = "Add Caption";
                }
                GetComponentInChildren<Text>().text = KeyBoardText;
            }
        }
    }


}
