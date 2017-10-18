using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManagerScript : MonoBehaviour {

    DrawingCanvas dc;
    CropCanvas cc;

    GameObject UpperCube;
    GameObject LowerCube;
    public GameObject CubePrefab;

    GameObject TLButtons;
    GameObject CropButtons;

    bool drawing;
    bool cropping;

    // Use this for initialization
	void Start () {
        drawing = false;
        cropping = false;
        cc = FindObjectOfType<CropCanvas>();
        dc = FindObjectOfType<DrawingCanvas>();
        CropButtons = GameObject.FindGameObjectWithTag("CroppingButtons");
        TLButtons= GameObject.FindGameObjectWithTag("TopLevelButtons");
        CropButtons.SetActive(false);
    }

    public void StartDraw()
    {
        drawing = true;
        cropping = false;
        dc.StartDraw();
    }

    public void StartCrop()
    {
        cropping = true;
        drawing = false;
        dc.GetComponent<CropCanvas>().StartCrop();

        CropButtons.SetActive(true);
        TLButtons.SetActive(false);

    }

    public void ApplyCrop()
    {
        cropping = false;
        cc.Apply();
        GameObject.FindGameObjectWithTag("CroppingButtons").SetActive(false);
        GameObject.FindGameObjectWithTag("TopLevelButtons").SetActive(true);
    }

    public void CancelCrop()
    {
        cropping = false;
        cc.Cancel();
        GameObject.FindGameObjectWithTag("CroppingButtons").SetActive(false);
        GameObject.FindGameObjectWithTag("TopLevelButtons").SetActive(true);
        dc.StartDraw();
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
