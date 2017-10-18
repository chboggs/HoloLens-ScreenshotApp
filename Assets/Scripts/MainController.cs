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
    
    void Start()
    {
        mm = GetComponent<ModeManager>();
        pc = GetComponent<PhotoCapturer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakePhoto();
        }
    }

    public void TakePhoto()
    {
        if (mm.currentMode != ModeManager.ModeManagerMode.Capture)
        {
            Debug.Log("not in capture mode");
            return;
        }

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
        currentImage = null;
        editedImage = null;
        mm.SetMode(ModeManager.ModeManagerMode.Capture);
    }

    public void SwitchToEdit()
    {
        if (mm.currentMode == ModeManager.ModeManagerMode.Preview || mm.currentMode == ModeManager.ModeManagerMode.Help)
        {
            editedImage = Instantiate(currentImage) as Texture2D;
            mm.SetMode(ModeManager.ModeManagerMode.Edit);
            //set canvas with editedImage
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Renderer>().material.mainTexture = editedImage;
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<DrawingCanvas>().tex = editedImage;
        }
    }

    public void SwitchToHelp()
    {
        mm.SetMode(ModeManager.ModeManagerMode.Help);
    }

    public void SwitchToGallery()
    {

        if (mm.currentMode == ModeManager.ModeManagerMode.Init || mm.currentMode == ModeManager.ModeManagerMode.Edit)
        {
            mm.SetMode(ModeManager.ModeManagerMode.Gallery);
        }
    }
}
