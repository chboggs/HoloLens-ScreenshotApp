using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeInputManager : MonoBehaviour
{
    public GazeReceiver focused;
    public bool gazing = true;
    public bool dragging;
    bool sentDragStart;
    bool endDrag;

    public GameObject cursor;

    public GameObject cameraReference;
    public Vector3 cameraLocation;
    Vector3 cameraDirection;
    Vector3 dragStartLocation;
    Vector3 dragChange;

    GameObject gestPos;
    //public GameObject cube;
    Vector3 dragDirectionFromCamera;

    public float DragStartDistance = 0.5f;

    public GestureRecognizer recognizer;

    void Start()
    {
        //gestPos = Instantiate(cube);

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
        if (gazing)
        {
            GazeReceiver newFocused = GetFocusedReceiver(r);
            SetGazeTarget(newFocused, r);
        }
        else if (dragging)
        {
            dragDirectionFromCamera = (dragStartLocation + dragChange) - cameraLocation;
            Ray gestureRay = new Ray(cameraLocation, dragDirectionFromCamera);
            Debug.DrawRay(gestureRay.origin, gestureRay.direction, Color.cyan);
            //gestPos.transform.position = dragStartLocation + dragChange;
            //Debug.LogFormat("end drag: {0}, sent: {1}", endDrag, sentDragStart);
            if (endDrag)
            {
                Debug.Log("Enddrag");
                StopDragging(focused, gestureRay);
                endDrag = false;
                dragging = false;
                gazing = true;
            }
            else if (!sentDragStart)
            {
                Debug.Log("start dragg");
                GazeReceiver newFocused = null;
                try
                {
                   newFocused = GetFocusedReceiver(gestureRay);
                }
                catch(Exception e)
                {
                    
                    Debug.Log(e);
                }
                if (newFocused != null)
                {
                    Debug.Log("Dragging on " + newFocused.gameObject.name);
                    sentDragStart = true;
                    StartDragging(newFocused, gestureRay);
                }
            }
            else
            {
                GetFocusedReceiver(gestureRay);
                if (focused != null) focused.Drag(gestureRay);
            }
        }

    }

    public void TapHandler(InteractionSourceKind source, int tapCount, Ray headRay)
    {
//        Debug.Log("tip tap");

        if (focused != null)
        {
            Debug.Log("tapped: " + focused.name);
            focused.Tapped(headRay);
        }
    }

    public void ManipulationStartedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("manipstart");
        gazing = false;
        dragging = true;
        sentDragStart = false;
        endDrag = false;
    }
    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {

        //Debug.Log("Dragging");
        dragChange = cumulativeDelta;

    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("manipcomplete");
        endDrag = true;
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

    void StartDragging(GazeReceiver gr, Ray ray)
    {
        SetGazeTarget(null, ray);
        dragStartLocation = cameraLocation + cameraDirection.normalized * DragStartDistance;
        if (gr != null)
        {
            gr.DragStart(ray);
        }
        focused = gr;

        //Debug.Log("gim start drag");
    }


    void StopDragging(GazeReceiver gr, Ray ray)
    {
        if (focused != null)
        {
            focused.DragEnd(ray);
        }
        //Debug.Log("gim end drag");
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
            //Debug.Log("thinks gaze focused changing");
            if (focused != null)
            {
                focused.GazeLeave(ray);
            }
            focused = gr;
            focused.GazeEnter(ray);
        }
    }
}