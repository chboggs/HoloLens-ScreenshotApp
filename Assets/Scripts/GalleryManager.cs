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
		photoIndex = -1;
		if(InitIndex ()){
			Texture2D sel = GetSelected();
			if(sel!=null){
				UpdateView(sel);
			}
		}
	}

	bool InitIndex(){
		if (photoIndex == -1) {
			photoIndex = main.CapturedPhotos.Count - 1;
		}
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
		cs.SetImage (GetSelected ());
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
