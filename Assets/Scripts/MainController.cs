﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    ModeManager mm;

	public List<Texture2D> CapturedPhotos;

    Texture2D currentImage;
    Texture2D editedImage;

    PhotoCapturer pc;
    
    void Start()
    {
		CapturedPhotos = new List<Texture2D> ();
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
		AddPhotoToRoll (photo);
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
			FindObjectOfType<EditManagerScript>().SetImage(editedImage);
        }
		if (mm.currentMode == ModeManager.ModeManagerMode.Gallery) {
            if(CapturedPhotos.Count == 0)
            {
                return;
            }
            Texture2D temp = FindObjectOfType<GalleryManager>().GetSelected();
            mm.SetMode (ModeManager.ModeManagerMode.Edit);
            FindObjectOfType<EditManagerScript>().SetImage(temp);

        }
        if (mm.currentMode == ModeManager.ModeManagerMode.Login)
        {
            mm.SetMode(ModeManager.ModeManagerMode.Edit);
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
			FindObjectOfType<GalleryManager> ().Reset ();
        }
    }

    public void SwitchToLogin()
    {
        mm.SetMode(ModeManager.ModeManagerMode.Login);
    }

	public void AddPhotoToRoll(Texture2D image){
		CapturedPhotos.Add (image);
	}

    public void OpenWebapp()
    {
        Application.OpenURL("http://screenshot-tool-eecs498.herokuapp.com");
    }
}
