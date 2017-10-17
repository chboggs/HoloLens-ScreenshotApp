// Starts a camera and assigns the texture to the current renderer.
// Pauses the camera when the "Pause" button is clicked and released.
using UnityEngine;
using System.Collections;

public class WebCamSetter : MonoBehaviour
{
    public WebCamTexture webcamTexture;

    void Start()
    {
        if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone))
        {
            Debug.Log("has auth");
        }
        webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }
}