using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GazeReceiver : MonoBehaviour {
	
	[System.Serializable]
	public class RayEvent : UnityEvent <Ray> {}

	public RayEvent GazeEnterEvent;
	public RayEvent GazeEvent;
	public RayEvent GazeLeaveEvent;

	public RayEvent TappedEvent;

	public RayEvent DragStartEvent;
	public RayEvent DragEndEvent;
	public RayEvent DragEvent;
	public RayEvent DragEnterEvent;
	public RayEvent DragLeaveEvent;


    public void GazeEnter(Ray gaze)
    {
        Debug.Log("enter");
        if (GazeEnterEvent != null) GazeEnterEvent.Invoke(gaze);
    }
    public void Gaze(Ray gaze)
    {
		if (GazeEvent != null) GazeEvent.Invoke(gaze);
    }

    public void GazeLeave(Ray gaze)
    {
        Debug.Log("leave");
		if (GazeLeaveEvent != null) GazeLeaveEvent.Invoke(gaze);
    }

    public void Tapped(Ray gaze)
    {
        Debug.Log("tap received");
		if (TappedEvent != null) TappedEvent.Invoke(gaze);
    }

    public void DragStart(Ray gaze)
    {
		if (DragStartEvent != null) DragStartEvent.Invoke(gaze);
    }

    public void Drag(Ray gaze)
    {
        //Debug.Log("receiver drag");
        if (DragEvent != null) DragEvent.Invoke(gaze);
        else Debug.Log("dragevent is null");
    }

    public void DragEnd(Ray gaze)
    {
		if (DragEndEvent != null) DragEndEvent.Invoke(gaze);
    }

    public void DragEnter(Ray gaze)
    {
		if (DragEnterEvent != null) DragEnterEvent.Invoke(gaze);
    }

    public void DragLeave(Ray gaze)
    {
		if (DragLeaveEvent != null) DragLeaveEvent.Invoke(gaze);
    }
}

