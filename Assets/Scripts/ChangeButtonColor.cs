using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButtonColor : MonoBehaviour {

    public Material HoverColor;
    public Material MainColor;

    // Use this for initialization
    private void Start()
    {
        GazeReceiver gr = GetComponent<GazeReceiver>();
        gr.GazeEnterEvent.AddListener(ChangeCursorOn);
        gr.GazeLeaveEvent.AddListener(ChangeCursorAway);
    }

    public void ChangeCursorOn (Ray r) {
        //Debug.Log("on");
        GetComponent<Renderer>().material = HoverColor;
	}
	
	// Update is called once per frame
	public void ChangeCursorAway (Ray r) {
        //Debug.Log("off");
        GetComponent<Renderer>().material = MainColor;
    }
}
