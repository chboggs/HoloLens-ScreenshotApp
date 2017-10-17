using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManagerScript : MonoBehaviour {

    DrawingCanvas dc;
    GameObject UpperCube;
    GameObject LowerCube;
    public GameObject CubePrefab;

    bool drawing;
    bool cropping;
    // Use this for initialization
	void Start () {
        drawing = false;
        cropping = false;
        dc = FindObjectOfType<DrawingCanvas>();
	}

    public void StartDraw()
    {
        drawing = true;
        cropping = false;
    }

    public void StartCrop()
    {
        cropping = true;
        drawing = false;
        
    }

    public void Apply()
    {
        cropping = false;
    }

    public void Cancel()
    {

    }

    public void Dragging(Ray r)
    {
        if (cropping == true)
        {
            
        }

        if (drawing == true)
        {
            dc.Draw(r);
        }
    }
}
