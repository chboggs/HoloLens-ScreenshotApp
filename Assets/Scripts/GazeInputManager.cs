using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeInputManager : MonoBehaviour
{
    public bool gazing = true;
    public bool dragging = false;
    public bool tapped = false;
    public GameObject cursor;
    public float DragTimeOut = 0.2f;

    public GameObject cameraReference;

    Vector3 cameraLocation;
    Vector3 cameraDirection;
    Vector3 dragChange;

    float lastDrag = 0;
    bool firstIgnored = false;


    public float DragStartDistance = 0.5f;
    public GestureRecognizer recognizer;

    void Start()
    {
        cameraReference = GameObject.FindGameObjectWithTag("MainCamera");

        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);

        recognizer.ManipulationStartedEvent += ManipulationStartedFunc;
        recognizer.ManipulationUpdatedEvent += ManipulationUpdatedFunc;
        recognizer.ManipulationCompletedEvent += ManipulationCompletedFunc;
        recognizer.TappedEvent += TapHandler;

        recognizer.StartCapturingGestures();

        StartCoroutine(Gazing());
        //Debug.Log("gaze starting");
    }

    IEnumerator Gazing()
    {
        Debug.Log("starting gazing");
        GazeReceiver focused = null;
        while (gazing)
        {
            //get gaze ray
            //Debug.Log("gazing");
            cameraLocation = cameraReference.transform.position;
            cameraDirection = cameraReference.transform.forward;
            Ray gazeRay = new Ray(cameraLocation, cameraDirection);

            GazeReceiver newFocused = GetFocusedReceiver(gazeRay);

            if (focused != newFocused)
            {
                if (focused != null)
                {
                    focused.GazeLeave(gazeRay);
                }
                if (newFocused != null)
                {
                    newFocused.GazeEnter(gazeRay);
                }
                focused = newFocused;
            }
            else if (focused != null)
            {
                focused.Gaze(gazeRay);
            }
            if (tapped)
            {
                tapped = false;
                if (focused != null)
                {
                    try
                    {
                        focused.Tapped(gazeRay);
                    }
                    catch(System.Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                }
            }

            yield return null;
        }
        //Debug.Log("gazing ending");

    }

    IEnumerator Dragging()
    {
        Debug.Log("starting dragging");

        cameraLocation = cameraReference.transform.position;
        cameraDirection = cameraReference.transform.forward;
        Ray gazeRay = new Ray(cameraLocation, cameraDirection);

        GazeReceiver startReceiver = GetFocusedReceiver(gazeRay);
        GazeReceiver gazedReceiver = startReceiver;

        Vector3 dragStartLocation = cameraLocation + cameraDirection.normalized * DragStartDistance;
        firstIgnored = false;
        if (startReceiver != null)
        {
            startReceiver.DragStart(gazeRay);
            Debug.LogFormat("starting drag on {0}", startReceiver.gameObject.name);
        }

        yield return null;

        yield return null;

        while (dragging)
        {
            cameraLocation = cameraReference.transform.position;
            Vector3 dragDirectionFromCamera = (dragStartLocation + dragChange) - cameraLocation;
            gazeRay = new Ray(cameraLocation, dragDirectionFromCamera);

            GazeReceiver newReceiver = GetFocusedReceiver(gazeRay);

            if (gazedReceiver != newReceiver)
            {
                if (gazedReceiver != null)
                {
                    gazedReceiver.GazeLeave(gazeRay);
                }
                if (newReceiver != null)
                {
                    newReceiver.GazeEnter(gazeRay);
                }
                gazedReceiver = newReceiver;
            }
            else if (gazedReceiver != null)
            {
                gazedReceiver.Drag(gazeRay);
            }

            if (startReceiver != gazedReceiver && startReceiver != null)
            {
                startReceiver.Drag(gazeRay);
            }

            if (Time.time - lastDrag > DragTimeOut)
            {
                dragging = false;
                Debug.Log("dragging timeout");
            }
            firstIgnored = true;
            yield return null;
        }

        if (startReceiver != null)
        {
            startReceiver.DragEnd(gazeRay);
        }
        if (gazedReceiver != null && gazedReceiver !=startReceiver)
        {
            gazedReceiver.DragEnd(gazeRay);
        }

        gazing = true;
        StartCoroutine(Gazing());
        Debug.Log("dragging ending");
    }

    public void TapHandler(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        tapped = true;
    }

    public void ManipulationStartedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("manip started");
        dragging = true;
        gazing = false;
        lastDrag = Time.time;
        StartCoroutine(Dragging());
    }

    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        dragChange = cumulativeDelta;
        lastDrag = Time.time;
    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("manip ended");
        dragging = false;
    }

    public GazeReceiver GetFocusedReceiver(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point;
            cursor.transform.up = hit.normal;
            return hit.collider.gameObject.GetComponent<GazeReceiver>();
        }
        else
        {
            cursor.transform.position = ray.origin + ray.direction.normalized * 5;
            cursor.transform.up = ray.direction;
        }
        return null;
    }
}