using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    GameObject camera;
    public float distance;

	// Use this for initialization
	void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
    private void Update()
    {
        transform.position = camera.transform.forward;
    }

}
