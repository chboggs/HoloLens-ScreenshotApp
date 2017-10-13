using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testgazeinput : MonoBehaviour {
    public bool gazed = false;
    public GameObject cube;
	// Use this for initialization
    public void Tapped()
    {
        Debug.Log("tapped");
    }
    public void Dragged(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * 100);
        Debug.Log("dragged");
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Debug.Log("hit!");
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == gameObject)
            {   
                Debug.Log("hit self");
                GameObject newcube = Instantiate(cube);

                newcube.transform.position = hit.point;

            }

        }
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
