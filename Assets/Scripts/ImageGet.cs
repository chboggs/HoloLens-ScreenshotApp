using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.VR.WSA.WebCam;
using System;

public class ImageGet : MonoBehaviour
{
    public PhotoCapture photoCaptureObject = null;
    public Texture2D targetTexture = null;

    bool taking = false;

    // Use this for initialization
    void Start()
    {        
        //Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        //targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Create a PhotoCapture object
        /*
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
                Debug.Log("create, photo is null: " + (photoCaptureObject == null).ToString());
                Debug.Log("here");
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
        */
        TakePhoto();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Capture();
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        Debug.Log("oncapturedphototomemory");
        // Copy the raw image data into the target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);
      
        // Create a GameObject to which the texture can be applied
        /*
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("Custom/Unlit/UnlitTexture"));

        quad.transform.parent = this.transform;
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

        quadRenderer.material.SetTexture("_MainTex", targetTexture);
        */
        gameObject.GetComponent<Renderer>().material.mainTexture = targetTexture;
        // Deactivate the camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        //photoCaptureObject.Dispose();
        //photoCaptureObject = null;
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        Debug.Log("disposed of photo");
        taking = false;
    }


    public void Capture()
    {
        Debug.Log("Capture");
        //photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        //photoCaptureObject.TakePhotoAsync("file",PhotoCaptureFileOutputFormat.JPG);
        //gameObject.GetComponent<Renderer>().material.mainTexture = targetTexture;
        Debug.Log("photo is null: " + (photoCaptureObject == null).ToString());
        if (taking == false)
        {
            TakePhoto();
        }

    }

    public void TakePhoto()
    {
        taking = true;
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
                Debug.Log("create, photo is null: " + (photoCaptureObject == null).ToString());
                Debug.Log("here");
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }
}