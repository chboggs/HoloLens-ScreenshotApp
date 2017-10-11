using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class ManipulationCoordinateTracker : MonoBehaviour
{

    static List<float> xCoords;
    static List<float> yCoords;
    static List<float> zCoords;

    public void ManipulationStartedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        xCoords.Add(headRay.origin.x);
        yCoords.Add(headRay.origin.y);
        zCoords.Add(headRay.origin.z);

        Debug.Log("Started");
        Debug.Log(headRay.origin.x);
        Debug.Log(headRay.origin.y);
        Debug.Log(headRay.origin.z);
    }
    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        xCoords.Add(headRay.origin.x);
        yCoords.Add(headRay.origin.y);
        zCoords.Add(headRay.origin.z);

        Debug.Log("Updated");
        Debug.Log(headRay.origin.x);
        Debug.Log(headRay.origin.y);
        Debug.Log(headRay.origin.z);
    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        xCoords.Add(headRay.origin.x);
        yCoords.Add(headRay.origin.y);
        zCoords.Add(headRay.origin.z);

        Debug.Log("Completed");
        Debug.Log(headRay.origin.x);
        Debug.Log(headRay.origin.y);
        Debug.Log(headRay.origin.z);
    }
    // Use this for initialization
    void Start()
    {
        xCoords = new List<float>();
        yCoords = new List<float>();
        zCoords = new List<float>();
        Debug.Log("Started the start");

        GestureRecognizer recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);

        recognizer.ManipulationStartedEvent += ManipulationStartedFunc;
        recognizer.ManipulationUpdatedEvent += ManipulationUpdatedFunc;
        recognizer.ManipulationCompletedEvent += ManipulationCompletedFunc;

        recognizer.StartCapturingGestures();
    }
}