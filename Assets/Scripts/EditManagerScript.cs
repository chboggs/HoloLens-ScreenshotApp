using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManagerScript : MonoBehaviour {

    DrawingCanvas dc;
    GameObject UpperCube;
    GameObject LowerCube;
    public GameObject CubePrefab;
    CropCanvas cc;

    bool drawing;
    bool cropping;
    // Use this for initialization
	void Start () {
        drawing = false;
        cropping = false;
        cc = FindObjectOfType<CropCanvas>();
        dc = FindObjectOfType<DrawingCanvas>();
        GameObject.FindGameObjectWithTag("CroppingButtons").SetActive(false);
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
        dc.GetComponent<CropCanvas>().MakeCubes();

        GameObject.FindGameObjectWithTag("CroppingButtons").SetActive(true);
        GameObject.FindGameObjectWithTag("TopLevelButtons").SetActive(false);

    }

    public void Apply()
    {
        cropping = false;
        dc.GetComponent<CropCanvas>().Apply();
        GameObject.FindGameObjectWithTag("CroppingButtons").SetActive(false);
        GameObject.FindGameObjectWithTag("TopLevelButtons").SetActive(true);
    }

    public void Cancel()
    {
        cropping = false;
        dc.GetComponent<CropCanvas>().Cancel();
        GameObject.FindGameObjectWithTag("CroppingButtons").SetActive(false);
        GameObject.FindGameObjectWithTag("TopLevelButtons").SetActive(true);
    }

    public void StartDragging(Ray r)
    {
        if (cropping)
        {
            cc.StartDrag(r);
        }
    }

    public void Dragging(Ray r)
    {
        if (cropping == true)
        {
            //Debug.DrawRay(r.origin, r.direction);
            cc.Dragging(r);
        }

        if (drawing == true)
        {
            dc.Draw(r);
        }
    }
}
