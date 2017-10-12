using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testgazeinput : MonoBehaviour {
    public bool gazed = false;
	// Use this for initialization
    public void Tapped()
    {
        Debug.Log("tapped");
    }
    public void Dragged()
    {
        Debug.Log("dragged");
    }

    public void GazeStart()
    {
        gazed = true;
    }
    public void GazeEnd()
    {
        gazed = false;
    }
}
