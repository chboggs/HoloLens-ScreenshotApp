using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageUploader : MonoBehaviour
{
    public string UploadRoute = "http://madeup.heroku.com/upload";

    public GameObject TextObject;

    private void Start()
    {
        TextObject = GameObject.FindGameObjectWithTag("MessageText");
        TextObject.SetActive(false);
    }

    public void StartUpload()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        Debug.Log("Starting Upload");

        if (!FindObjectOfType<LoginManager>().HasLogin())
        {
            FindObjectOfType<ModeManager>().SetMode(ModeManager.ModeManagerMode.Login);
        }

        Texture2D image;
        string caption;
        try
        {
             image = GetComponent<Renderer>().material.mainTexture as Texture2D;
             caption = FindObjectOfType<TakeTextInput>().KeyBoardText;
        }

        catch(Exception e)
        {
            Debug.Log(e);
            yield break;
        }
        Debug.Log("Caption is: " + caption);
        WWWForm form = new WWWForm();
        byte[] rawImage = image.EncodeToPNG();
        Debug.Log("uploading");
        form.AddBinaryData("image", rawImage, "screenshot.png", "image/png");
        form.AddField("body", caption);
        form.AddField("username", FindObjectOfType<LoginManager>().GetUsername());
        form.AddField("password", FindObjectOfType<LoginManager>().GetPassword());

        WWW w = new WWW(UploadRoute, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            StartCoroutine(ShowMessage(w.error));
        }
        else
        {
            print("Finished Uploading Screenshot");
            StartCoroutine(ShowMessage("Uploaded Successfully"));
        }
    }

    IEnumerator ShowMessage(string message)
    {
        Debug.Log("SHOWING MESSAGE");
        TextObject.SetActive(true);
        TextObject.GetComponent<Text>().text = message;
        yield return new WaitForSeconds(3);
        TextObject.SetActive(false);

    }
}