using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    ModeManager mm;

    Texture2D currentImage;
    Texture2D editedImage;

    PhotoCapturer pc;


    // Use this for initialization
    void Start()
    {
        mm = GetComponent<ModeManager>();
        pc = GetComponent<PhotoCapturer>();
    }

    public void TakePhoto()
    {
        Debug.Log("takephoto");
        if (mm.currentMode != ModeManager.ModeManagerMode.Capture)
        {
            Debug.Log("not in correct mode");
            return;
        }
        //take photo
        //currentImage = ****

        Debug.Log("start take photo");

        pc.StartTakePhoto(this);        

    }

    public void GetPhoto(Texture2D photo)
    {
        Debug.Log("got photo");
        mm.SetMode(ModeManager.ModeManagerMode.Preview);
        try
        {
            GameObject.FindGameObjectWithTag("PictureFrame").GetComponent<Renderer>().material.mainTexture = photo;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        currentImage = photo;
    }

    public void SwitchToCapture()
    {
        /*
        if(currentMode == ModeManager.ModeManagerMode.Preview)
        {
            */
        //throwing out image
        currentImage = null;
        editedImage = null;
        mm.SetMode(ModeManager.ModeManagerMode.Capture);
        /*
        }
        else if(currentMode == ModeManager.ModeManagerMode.Edit)
        {
            currentImage = null;

        }
        */
    }
    public void SwitchToEdit()
    {
        if (mm.currentMode == ModeManager.ModeManagerMode.Preview)
        {
            editedImage = Instantiate(currentImage) as Texture2D;
            mm.SetMode(ModeManager.ModeManagerMode.Edit);
            //set canvas with editedImage
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Renderer>().material.mainTexture = editedImage;
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<DrawingCanvas>().tex = editedImage;
        }
    }
}
