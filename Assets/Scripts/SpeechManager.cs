using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    KeywordRecognizer buttonRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    Dictionary<string, GameObject> buttonWords = new Dictionary<string, GameObject>();

    MainController mc;

    // Use this for initialization
    void Start()
    {
        Debug.Log("registering buttons");
        foreach (KeywordAssigner assigner in FindObjectsOfType<KeywordAssigner>())
        {
            if (assigner.UseKeyWord && !buttonWords.ContainsKey(assigner.GetKeyWord())){
                Debug.LogFormat("Registering button: {0}", assigner.gameObject.name);

                buttonWords.Add(assigner.GetKeyWord(), assigner.gameObject);
            }
        }
        Debug.Log("done registering buttons");

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

        keywords.Add("Gallery", () =>
        {
            Debug.Log("To Gallery said");
            mc.SwitchToGallery();

        });

        keywords.Add("Help", () =>
        {
            Debug.Log("Help said");
            mc.SwitchToHelp();
        });

        keywords.Add("crack", () =>
        {
            Debug.Log("crack said");
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        
        //buttonRecognizer = new KeywordRecognizer(buttonWords.Keys.ToArray());
        //buttonRecognizer.OnPhraseRecognized += ButtonRecognized;
        //buttonRecognizer.Start();
        
        Debug.Log("Started speech");
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

    void ButtonRecognized(PhraseRecognizedEventArgs args)
    {
        GameObject button;
        Debug.Log("button recognized: " + args.text);
        if (buttonWords.TryGetValue(args.text, out button))
        {
            InvokeButton(button);
        }
    }

    void InvokeButton(GameObject button)
    {
        if (button.activeSelf)
        {
            button.GetComponent<GazeReceiver>().Tapped(new Ray(Vector3.zero, Vector3.zero));
        }
    }
}