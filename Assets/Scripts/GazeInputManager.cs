using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeInputManager : MonoBehaviour
{
    GazeReceiver focused;
    bool gazing;
    bool dragging;
    bool sentDragStart;

    GameObject cameraReference;
    Vector3 cameraLocation;
    Vector3 cameraDirection;

    public GestureRecognizer recognizer;

    void Start()
    {
        cameraReference = GameObject.FindGameObjectWithTag("MainCamera");

        Debug.Log("Started the start");

        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);

        recognizer.ManipulationStartedEvent += ManipulationStartedFunc;
        recognizer.ManipulationUpdatedEvent += ManipulationUpdatedFunc;
        recognizer.ManipulationCompletedEvent += ManipulationCompletedFunc;

        recognizer.TappedEvent += TapHandler;

        recognizer.StartCapturingGestures();
    }

    private void Update()
    {
        //get camera location 
        cameraLocation = cameraReference.transform.position;
        cameraDirection = cameraReference.transform.forward;
        Ray r = new Ray(cameraLocation, cameraDirection);
        RaycastHit hit;
        GazeReceiver newFocused = null;
        if (Physics.Raycast(cameraLocation, cameraDirection, out hit))
        {
            newFocused = hit.collider.gameObject.GetComponent<GazeReceiver>();
        }
        SetGazeTarget(newFocused, r);
    }

    public void TapHandler(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Debug.Log("tip tap");
        if (focused != null)
        {
            focused.Tapped(headRay);
        }
    }

    public void ManipulationStartedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {

    }
    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
    }

    void SetGazeTarget(GazeReceiver gr, Ray ray)
    {
        if (gr == null)
        {
            if (focused != null)
            {
                focused.GazeLeave(ray);
            }
            focused = null;
        }
        else if (gr == focused)
        {
            gr.Gaze(ray);
        }
        else
        {
            if (focused != null)
            {
                focused.GazeLeave(ray);
            }
            focused = gr;
            focused.GazeEnter(ray);
        }
    }
}