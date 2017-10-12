using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeReceiver : MonoBehaviour {
    
    public delegate void GazeDelegate(Ray gaze);

    public event GazeDelegate GazeEnterEvent;
    public event GazeDelegate GazeEvent;
    public event GazeDelegate GazeLeaveEvent;

    public event GazeDelegate TappedEvent;

    public event GazeDelegate DragStartEvent;
    public event GazeDelegate DragEndEvent;
    public event GazeDelegate DragEvent;
    public event GazeDelegate DragEnterEvent;
    public event GazeDelegate DragLeaveEvent;


    public void GazeEnter(Ray gaze)
    {
        if (GazeEnterEvent != null) GazeEnterEvent(gaze);
    }
    public void Gaze(Ray gaze)
    {
        if (GazeEvent != null) GazeEvent(gaze);
    }

    public void GazeLeave(Ray gaze)
    {
        if (GazeLeaveEvent != null) GazeLeaveEvent(gaze);
    }

    public void Tapped(Ray gaze)
    {
        if (TappedEvent != null) TappedEvent(gaze);
    }

    public void DragStart(Ray gaze)
    {
        if (DragStartEvent != null) DragStartEvent(gaze);
    }

    public void Drag(Ray gaze)
    {
        if (DragEvent != null) DragEvent(gaze);
    }

    public void DragEnd(Ray gaze)
    {
        if (DragEndEvent != null) DragEndEvent(gaze);
    }

    public void DragEnter(Ray gaze)
    {
        if (DragEnterEvent != null) DragEnterEvent(gaze);
    }

    public void DragLeave(Ray gaze)
    {
        if (DragLeaveEvent != null) DragLeaveEvent(gaze);
    }
}

