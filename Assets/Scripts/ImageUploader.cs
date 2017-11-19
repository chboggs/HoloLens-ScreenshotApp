using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageUploader : MonoBehaviour
{
    public LoginManager loginmanager;
    public EditManagerScript editmanager;
    public string UploadRoute = "https://backend-498.herokuapp.com/api/new-image-hololens";

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
        Debug.Log("here");
        Texture2D image;
        try
        {
             image = GetComponent<CanvasSizer>().GetImage();
        }

        catch(Exception e)
        {
            Debug.Log(e);
            yield break;
        }
        Debug.LogFormat("Editman null {0}", editmanager == null);
        editmanager.SaveToGallery();
        Debug.Log("saved to gal");
        WWWForm form = new WWWForm();
        byte[] rawImage = image.EncodeToPNG();
        form.AddBinaryData("image", rawImage, "screenshot.png", "image/png");
        form.AddField("username", loginmanager.GetUsername());
        form.AddField("password", loginmanager.GetPassword());
        Debug.Log("form made");
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