using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{

    int photoIndex = -1;
    public Text placeText;
    MainController main;
    CanvasSizer cs;

    // Use this for initialization
    void Awake()
    {
        main = FindObjectOfType<MainController>();
        cs = GetComponentInChildren<CanvasSizer>();
        SetPlaceText();
    }

    public void Reset()
    {
        Debug.Log("reset");
        photoIndex = -1;
        InitIndex();
        UpdateView();
    }

    bool InitIndex()
    {
        Debug.Log("starting init");
        if (photoIndex == -1)
        {
            try
            {
                photoIndex = main.CapturedPhotos.Count - 1;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            
        }
        Debug.Log("init, index: " + photoIndex.ToString());
        SetPlaceText();
        return photoIndex >= 0;
    }

    public void Previous()
    {
        Debug.Log(main.CapturedPhotos.Count);
        photoIndex = Mathf.Max(0, photoIndex - 1);
        UpdateView();
    }

    public void Next()
    {
        Debug.Log(main.CapturedPhotos.Count);
        photoIndex = Mathf.Min(main.CapturedPhotos.Count - 1, photoIndex + 1);
        UpdateView();
    }

    public void Delete()
    {
        if (GetSelected() != null)
        {
            main.CapturedPhotos.RemoveAt(photoIndex);
            photoIndex = Mathf.Clamp(photoIndex, 0, main.CapturedPhotos.Count - 1);
            UpdateView();
        }
    }

    void UpdateView()
    {
        //Debug.Log("update");
        Texture2D sel = GetSelected();
        if (sel != null)
        {
            cs.gameObject.SetActive(true);
            Debug.Log("not null");
            cs.SetImage(sel);
        }
        else
        {
            Debug.Log("null");
            cs.gameObject.SetActive(false);
        }
        SetPlaceText();
    }

    public Texture2D GetSelected()
    {
        if (photoIndex >= 0 && photoIndex < main.CapturedPhotos.Count)
        {
            return main.CapturedPhotos[photoIndex];
        }
        else
        {
            return null;
        }
    }

    void SetPlaceText()
    {
        if (main.CapturedPhotos.Count > 0)
        {
            placeText.text = (photoIndex + 1).ToString() + " of " + main.CapturedPhotos.Count.ToString();
        }
        else
        {
            placeText.text = "0 of 0";
        }
        
    }

}
