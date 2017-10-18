using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                    GetComponentInChildren<TextMesh>().text = "Edit Caption";
                }
                else
                {
                    GetComponentInChildren<TextMesh>().text = "Add Caption";
                }
            }
        }
    }


}
