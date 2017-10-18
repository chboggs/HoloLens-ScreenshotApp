using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    MainController mc;

    // Use this for initialization
    void Start()
    {
        mc = GameObject.FindGameObjectWithTag("Controller").GetComponent<MainController>();
        keywords.Add("Test", () =>
        {
            // Call the OnReset method on every descendant object.
            Debug.Log("test");
        });
        
        keywords.Add("Capture Photo", () =>
        {
            Debug.Log("photo");
            mc.TakePhoto();
        });

        keywords.Add("Take New Photo", () =>
        {
            Debug.Log("Take New Photo said");
            mc.SwitchToCapture();
        });

        keywords.Add("Save and Edit", () =>
        {
            Debug.Log("Edit said");
            mc.SwitchToEdit();
        });
        /*
        keywords.Add("To Gallery", () =>
        {
            Debug.Log("To Gallery said");
            GameObject.FindGameObjectWithTag("Controller").GetComponent<MainController>().SwitchToGallery();

        });
        */
        keywords.Add("Draw", () =>
        {
            Debug.Log("Draw said");
            GameObject.FindGameObjectWithTag("EditManager").GetComponent<EditManagerScript>().StartDraw();

        });

        keywords.Add("Crop", () =>
        {
            Debug.Log("Crop said");
            GameObject.FindGameObjectWithTag("EditManager").GetComponent<EditManagerScript>().StartCrop();

        });

        keywords.Add("Apply", () =>
        {
            Debug.Log("Apply said");
            GameObject.FindGameObjectWithTag("EditManager").GetComponent<EditManagerScript>().ApplyCrop();

        });

        keywords.Add("Cancel", () =>
        {
            Debug.Log("Cancel said");
            GameObject.FindGameObjectWithTag("EditManager").GetComponent<EditManagerScript>().CancelCrop();

        });

        keywords.Add("Save and Share", () =>
        {
            Debug.Log("Save and share said");
            GameObject.FindGameObjectWithTag("DrawingCanvas").GetComponent<ImageUploader>().StartUpload();

        });

        keywords.Add("Back", () =>
        {
            Debug.Log("Back said");
            GameObject.FindGameObjectWithTag("GalleryController").GetComponent<GalleryControllerScript>().Back();

        });

        keywords.Add("Next", () =>
        {
            Debug.Log("Next said");
            GameObject.FindGameObjectWithTag("GalleryController").GetComponent<GalleryControllerScript>().Next();

        });

        keywords.Add("Add caption", () =>
        {
            Debug.Log("adding caption");
            GameObject.FindObjectOfType<TakeTextInput>().TakeKeyboardInput();

        });

        keywords.Add("Edit caption", () =>
        {
            Debug.Log("adding caption");
            GameObject.FindObjectOfType<TakeTextInput>().TakeKeyboardInput();
        });

        keywords.Add("Help", () =>
        {
            Debug.Log("adding caption");
            mc.SwitchToHelp();
        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        Debug.Log("started voice recog");
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("recognized");
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}