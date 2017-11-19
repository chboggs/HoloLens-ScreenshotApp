using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButtonColor : MonoBehaviour {

    public Material HoverColor;
    public Material MainColor;
    public Material TappedColor;

    bool hovered = false;

    // Use this for initialization
    private void Start()
    {
        GazeReceiver gr = GetComponent<GazeReceiver>();
        gr.GazeEnterEvent.AddListener(ChangeCursorOn);
        gr.GazeLeaveEvent.AddListener(ChangeCursorAway);
        gr.TappedEvent.AddListener(Tapped);
    }

    public void ChangeCursorOn (Ray r) {
        //Debug.Log("on");
        GetComponent<Renderer>().material = HoverColor;
        hovered = true;
	}
	
	// Update is called once per frame
	public void ChangeCursorAway (Ray r) {
        //Debug.Log("off");
        GetComponent<Renderer>().material = MainColor;
        hovered = false;
    }

    public void Tapped(Ray r)
    {
        StartCoroutine(DoTapped());
    }

    IEnumerator DoTapped()
    {
        GetComponent<Renderer>().material = TappedColor;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material = hovered?HoverColor:MainColor;
    }
}
