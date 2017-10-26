using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour {

	int photoIndex = -1;

	MainController main;
	CanvasSizer cs;

	// Use this for initialization
	void Start () {
		main = FindObjectOfType<MainController> ();
		cs = GetComponentInChildren<CanvasSizer> ();
	}

	public void Reset(){
        Debug.Log("reset");
		photoIndex = -1;
		if(InitIndex ()){
            UpdateView();
		}
	}

	bool InitIndex(){
		if (photoIndex == -1) {
            List<Texture2D> cc = main.CapturedPhotos;
			photoIndex = cc.Count - 1;
		}
        Debug.Log("init, index: " + photoIndex.ToString());
		return photoIndex >= 0;
	}

	public void Previous(){
		photoIndex = Mathf.Max (0, photoIndex - 1);
		UpdateView();
	}

	public void Next(){
	    photoIndex = Mathf.Min (main.CapturedPhotos.Count - 1, photoIndex + 1);
		UpdateView();
	}

	void UpdateView(){
        Debug.Log("update");
        Texture2D sel = GetSelected();
        if (sel != null)
        {
            Debug.Log("not null");
            cs.SetImage(sel);
        }
        else
        {
            Debug.Log("null");
        }
	}

	public Texture2D GetSelected(){
		if (photoIndex >= 0 && photoIndex < main.CapturedPhotos.Count) {
			return main.CapturedPhotos [photoIndex];
		}
		else {
			return null;
		}
	}

}
