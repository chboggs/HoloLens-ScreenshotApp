using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManagerScript : MonoBehaviour
{

    DrawingCanvas dc;
    CropCanvas cc;

    GameObject UpperCube;
    GameObject LowerCube;
    public GameObject CubePrefab;

    GameObject TLButtons;
    GameObject CropButtons;

    public bool drawing =true;
	public bool cropping = false;

    Stack<Texture2D> UndoStack;

    // Use this for initialization
    void Start()
    {
        cc = FindObjectOfType<CropCanvas>();
        dc = FindObjectOfType<DrawingCanvas>();
        CropButtons = GameObject.FindGameObjectWithTag("CroppingButtons");
        TLButtons = GameObject.FindGameObjectWithTag("TopLevelButtons");
    }

	public void SetImage(Texture2D image){

        SetCurrentTexture(image);
		UndoStack = new Stack<Texture2D>();
		StartDraw();
		CropButtons.SetActive(false);
		TLButtons.SetActive (true);
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
        AddToStack();
        CropButtons.SetActive(true);
        TLButtons.SetActive(false);
    }

    public void ApplyCrop()
    {
        cropping = false;
        cc.Apply();
        CropButtons.SetActive(false);
        TLButtons.SetActive(true);
        StartDraw();
    }

    public void CancelCrop()
    {
        cropping = false;
        cc.Cancel();
        CropButtons.SetActive(false);
        TLButtons.SetActive(true);
        StartDraw();
    }

    public Texture2D GetCurrentTexture()
    {
        Texture2D current = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        Debug.Log("got current");
		Texture2D currentCopy = Instantiate (current) as Texture2D;
        return currentCopy;
    }

    void AddToStack()
    {
        UndoStack.Push(GetCurrentTexture());
        Debug.Log("added to stack");
    }

    public void Undo()
    {
        if (UndoStack.Count > 0)
        {
            Debug.Log("undoing");
            Texture2D restored = UndoStack.Pop();
            SetCurrentTexture(restored);
        }
    }

    void SetCurrentTexture(Texture2D tex)
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<MeshRenderer>().material.mainTexture = tex;
        dc.tex = tex;
    }

    public void StartDragging(Ray r)
    {
        Debug.Log("start drag edit");
        if (drawing)
        {
            AddToStack();
            dc.StartDraw();
        }
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
            Debug.Log("Dragdraw");
            dc.Draw(r);
        }
    }

	public void EndDragging(Ray r){
		Debug.Log ("recieved enddraw");
		if (drawing) {
			dc.Apply ();
		}
	}
}
