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
        Debug.Log("dragged");
        Debug.DrawRay(ray.origin, ray.direction);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
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
