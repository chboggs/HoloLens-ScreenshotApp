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

        Console.Write("Started");
        Console.Write(headRay.origin.x);
        Console.Write(headRay.origin.y);
        Console.Write(headRay.origin.z);
    }
    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        xCoords.Add(headRay.origin.x);
        yCoords.Add(headRay.origin.y);
        zCoords.Add(headRay.origin.z);

        Console.Write("Updated");
        Console.Write(headRay.origin.x);
        Console.Write(headRay.origin.y);
        Console.Write(headRay.origin.z);
    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        xCoords.Add(headRay.origin.x);
        yCoords.Add(headRay.origin.y);
        zCoords.Add(headRay.origin.z);

        Console.Write("Completed");
        Console.Write(headRay.origin.x);
        Console.Write(headRay.origin.y);
        Console.Write(headRay.origin.z);
    }
    // Use this for initialization
    void Start()
    {
        xCoords = new List<float>();
        yCoords = new List<float>();
        zCoords = new List<float>();

        GestureRecognizer recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);

        recognizer.ManipulationStartedEvent += ManipulationStartedFunc;
        recognizer.ManipulationUpdatedEvent += ManipulationUpdatedFunc;
        recognizer.ManipulationCompletedEvent += ManipulationCompletedFunc;

        recognizer.StartCapturingGestures();
    }
}