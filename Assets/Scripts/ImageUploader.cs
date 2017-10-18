using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class ImageUploader : MonoBehaviour
{
    public string UploadRoute = "http://madeup.heroku.com/upload";

    GameObject TextObject;

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
        Texture2D image = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        string caption = GetComponent<TakeTextInput>().KeyBoardText;

        WWWForm form = new WWWForm();
        byte[] rawImage = image.EncodeToPNG();
        Debug.Log("uploading");
        form.AddBinaryData("image", rawImage, "screenshot.png", "image/png");
        form.AddField("caption", caption);

        WWW w = new WWW(UploadRoute, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            ShowMessage(w.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
            ShowMessage("Uploaded Successfully");
        }
    }

    IEnumerator ShowMessage(string message)
    {
        TextObject.SetActive(true);
        TextObject.GetComponent<Text>().text = message;
        yield return new WaitForSeconds(3);
        TextObject.SetActive(false);

    }
}