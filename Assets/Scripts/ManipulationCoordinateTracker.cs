using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class ManipulationCoordinateTracker : MonoBehaviour
{
    public GameObject cube;
    public List<Vector3> coordinates;
    Vector3 start;

    public void MyTapEventHandler(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Debug.Log("tip tap");
    }

    public void ManipulationStartedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        coordinates.Add(headRay.origin);

        Debug.Log("Started");
        Debug.Log(headRay.direction.ToString("F4"));
 
        

        //Debug.Log(cumulativeDelta);

    }
    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        coordinates.Add(headRay.origin);
        //Debug.Log(cumulativeDelta.ToString("F4"));
        Debug.Log(headRay.direction.ToString("F4"));
        Spawn((cumulativeDelta) + headRay.origin +headRay.direction.normalized*2);

        //Debug.Log("Updated");
    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        coordinates = new List<Vector3>();

        //Debug.Log("Completed");
    }
    
    // Use this for initialization

    GestureRecognizer recognizer;

    void Spawn(Vector3 loc)
    {
        GameObject newcube = Instantiate(cube);
        newcube.transform.position = loc;
    }

    void Start()
    {
       
        Debug.Log("Started the start");

        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);
        
        recognizer.ManipulationStartedEvent += ManipulationStartedFunc;
        recognizer.ManipulationUpdatedEvent += ManipulationUpdatedFunc;
        recognizer.ManipulationCompletedEvent += ManipulationCompletedFunc;
        
        recognizer.TappedEvent += (InteractionSourceKind source, int tapCount, Ray headRay) =>
        {
            Debug.Log("tapped");
            Debug.Log(headRay.direction.ToString("F4"));
        };

        recognizer.StartCapturingGestures();
    }
}