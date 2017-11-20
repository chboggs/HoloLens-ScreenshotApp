using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.VR.WSA.WebCam;
using System;

public class PhotoCapturer : MonoBehaviour
{
    public PhotoCapture photoCaptureObject = null;
    public Texture2D targetTexture = null;

    bool taking = false;

    MainController mc;

    public void StartTakePhoto(MainController mci)
    {
        if (taking)
        {
            return;
        }
        taking = true;
        FindObjectOfType<Flasher>().Flash();

        //Debug.Log("starting to take photo");
        mc = mci;
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            Debug.Log("createasync");
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                //Debug.Log("create, photo is null: " + (photoCaptureObject == null).ToString());
                //Debug.Log("here");
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        //Debug.Log("copying to memory");
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        mc.GetPhoto(targetTexture);
        //Debug.Log("done");
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        //Debug.Log("disposed of photo");
        taking = false;
    }
}