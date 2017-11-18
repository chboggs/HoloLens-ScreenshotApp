using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageUploader : MonoBehaviour
{
    public LoginManager loginmanager;
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

        if (!loginmanager.HasLogin())
        {
            Debug.Log("Not Logged in, switching mode");
            FindObjectOfType<ModeManager>().SetMode(ModeManager.ModeManagerMode.Login);
            yield break;
        }

        Texture2D image;
        string caption;
        try
        {
             image = GetComponent<CanvasSizer>().GetImage();
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
        form.AddBinaryData("image", rawImage, "screenshot.png", "image/png");
        form.AddField("body", caption);
        form.AddField("username", loginmanager.GetUsername());
        form.AddField("password", loginmanager.GetPassword());
        WWW w = new WWW(UploadRoute, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            Debug.Log(w.error);
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