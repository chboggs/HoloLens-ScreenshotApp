using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ImageUploader : MonoBehaviour
{
    public LoginManager loginmanager;
    public EditManagerScript editmanager;
    public string UploadRoute = "https://backend-498.herokuapp.com/api/new-image-hololens";

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

        string response = w.text;

        var parsed = SimpleJSON.JSON.Parse(response);

        string msg = parsed["msg"];
        string url = parsed["url"];

        if (string.IsNullOrEmpty(msg)){
            msg = "Bad server response";
            url = null;
        };

        Resources.FindObjectsOfTypeAll<ImageUploaded>()[0].SetInfo(msg, url);
    }
}