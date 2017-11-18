using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    public GameObject UserBox;
    public GameObject PassBox;

    public bool HasLogin()
    {
        Debug.Log("checking for login");
        return UserBox.GetComponent<Text>().text.Length != 0 && PassBox.GetComponent<Text>().text.Length != 0;
    }

    public string GetUsername()
    {
        return UserBox.GetComponent<Text>().text;
    }

    public string GetPassword()
    {
        return PassBox.GetComponent<Text>().text;
    }
}
