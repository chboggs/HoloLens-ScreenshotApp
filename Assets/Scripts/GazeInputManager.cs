﻿using System.Collections;
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
	bool endDrag;

    GameObject cameraReference;
    Vector3 cameraLocation;
    Vector3 cameraDirection;
	Vector3 dragStartLocation;
	Vector3 dragChange;

	Vector3 dragDirectionFromCamera;

	public float DragStartDistance = 0.5f;

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
		if (gazing) {
			GazeReceiver newFocused = GetFocusedReceiver (r);
			SetGazeTarget (newFocused, r);
		} 
		else if (dragging) {
			dragDirectionFromCamera = (dragStartLocation + dragChange) - cameraLocation;
			Ray gestureRay = new Ray (cameraLocation, dragDirectionFromCamera);
			GazeReceiver newFocused = GetFocusedReceiver (gestureRay);
			if (endDrag) {
				StopDragging (newFocused, gestureRay);
			} else if (!sentDragStart) {
				StartDragging (newFocused, gestureRay);
			} else {
				SetDragTarget (newFocused, gestureRay);
			}
		}

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
		gazing = false;
		dragging = true;
		sentDragStart = false;
    }
    public void ManipulationUpdatedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
		if (!sentDragStart) {
			dragStartLocation = headRay.origin + headRay.direction.normalized * DragStartDistance;
		}
		dragChange = cumulativeDelta;
    }

    public void ManipulationCompletedFunc(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
		endDrag = true;
    }

	public GazeReceiver GetFocusedReceiver(Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast (cameraLocation, cameraDirection, out hit)) {
			return hit.collider.gameObject.GetComponent<GazeReceiver> ();
		}
		return null;
	}

	void StartDragging(GazeReceiver gr, Ray ray){
		SetGazeTarget (null);
		if (gr != null) {
			gr.DragStart (ray);
			focused = gr;
		}
	}

	void SetDragTarget(GazeReceiver gr, Ray ray){

		focused.Drag (ray);
		/*
		if (gr == null) {
			if (focused != null) {
				focused.DragLeave(ray);
			}
			focused = null;
		} else if (gr == focused) {
			gr.Drag (ray);
		} else {
			if (focused != null) {
				focused.DragLeave(ray);
			}
			focused = gr;
			focused.DragEnter();
		}
		*/
	}

	void StopDragging(GazeReceiver gr, Ray ray){
		if (focused != null) {
			focused.DragEnd ();
		}
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