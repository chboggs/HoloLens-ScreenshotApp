using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButtonTracker : MonoBehaviour {
    /*
    GameObject Next = GameObject.FindGameObjectWithTag("HelpNext");
    GameObject Back = GameObject.FindGameObjectWithTag("HelpBack");
    GameObject Overall = GameObject.FindGameObjectWithTag("HelpOverall");
    GameObject Drawing = GameObject.FindGameObjectWithTag("HelpDrawing");
    GameObject Cropping = GameObject.FindGameObjectWithTag("HelpCropping");
    GameObject Captions = GameObject.FindGameObjectWithTag("HelpCaptions");
    */
    public GameObject[] Pages;

    int currentPage = 0;

    void Start () {
        foreach(GameObject g in Pages)
        {
            g.SetActive(false);
        }
        Pages[currentPage].SetActive(true);
    }
	
	public void HelpNextClick () {
        Pages[currentPage].SetActive(false);
        currentPage = Mathf.Min(Pages.Length - 1, currentPage + 1);
        Pages[currentPage].SetActive(true);
        /*
        if (Overall.activeSelf)
        {
            Back.SetActive(true);
            Drawing.SetActive(true);
            Overall.SetActive(false);
        }
        if (Drawing.activeSelf)
        {
            Cropping.SetActive(true);
            Drawing.SetActive(false);
        }
        if (Cropping.activeSelf)
        {
            Captions.SetActive(true);
            Cropping.SetActive(false);
            Next.SetActive(false);

        }
        */
    }

    public void HelpBackClick()
    {
        Pages[currentPage].SetActive(false);
        currentPage = Mathf.Max(0, currentPage - 1);
        Pages[currentPage].SetActive(true);
        /*
        if (Drawing.activeSelf)
        {
            Overall.SetActive(true);
            Drawing.SetActive(false);
            Back.SetActive(false);
        }
        if (Cropping.activeSelf)
        {
            Drawing.SetActive(true);
            Cropping.SetActive(false);

        }
        if (Captions.activeSelf)
        {
            Captions.SetActive(false);
            Cropping.SetActive(true);
            Next.SetActive(true);
        }
        */
    }
}
