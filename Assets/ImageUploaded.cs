using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageUploaded : MonoBehaviour {

    public string url;
    public string message;

    bool active = false;
    public void SetInfo(string messageIn, string urlIn)
    {
        gameObject.SetActive(true);
        active = true;
        message = messageIn;
        url = urlIn;
        transform.Find("Canvas/Text").GetComponent<Text>().text = message;

        if(url == null)
        {
            transform.Find("OpenInNew").gameObject.SetActive(false);
            transform.Find("Dismiss").localPosition = new Vector3(0, -0.15f, 0);
        }
        else
        {
            transform.Find("OpenInNew").gameObject.SetActive(true);
            transform.Find("Dismiss").localPosition = new Vector3(-0.17f, -0.15f, 0);
        }

    }

    public void Dismiss()
    {
        gameObject.SetActive(false);
        active = false;
    }

    public void OpenInBrowser()
    {
        if (active)
        {
            if (url != null)
            {
                Application.OpenURL(url);
            }
            Dismiss();
        }
    }


}
