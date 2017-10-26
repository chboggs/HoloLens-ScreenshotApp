using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButtonColor : MonoBehaviour {

    // Use this for initialization
    private void Start()
    {
        GazeReceiver gr = GetComponent<GazeReceiver>();
        gr.GazeEnterEvent.AddListener(ChangeCursorOn);
        gr.GazeLeaveEvent.AddListener(ChangeCursorAway);
    }

    public void ChangeCursorOn (Ray r) {
        GetComponent<Renderer>().material.SetColor("blue", Color.blue);
	}
	
	// Update is called once per frame
	public void ChangeCursorAway (Ray r) {
        GetComponent<Renderer>().material.SetColor("white", Color.white);
    }
}
