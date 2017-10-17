using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ImageUploader : MonoBehaviour
{

    public string UploadRoute = "http://madeup.heroku.com/upload";
    string temporaryobjecttofixerror;

    public void StartUpload()
    {
        StartCoroutine(Upload());
    }
    IEnumerator Upload()
    {

        Texture2D image = FindObjectOfType<DrawingCanvas>().tex;

        WWWForm form = new WWWForm();
        byte[] rawImage = image.EncodeToPNG();
        Debug.Log("uploading");
        form.AddBinaryData("image", rawImage, "screenshot.png", "image/png");

        WWW w = new WWW(UploadRoute, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }
    }
}