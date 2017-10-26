using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSizer : MonoBehaviour {

	MeshRenderer mr;
	float maxWidth;
	float maxHeight;

	// Use this for initialization
	void Start () {
		mr = GetComponent<MeshRenderer> (); 
	}

	public void SetImage(Texture2D image){
		mr.material.mainTexture = image;

		float canvasRatio = maxWidth / maxHeight;
		float imageRatio = (float)image.width / image.height;

		float newWidth, newHeight;

		if (canvasRatio < imageRatio) {
			newWidth = maxWidth;
			newHeight = maxWidth / imageRatio;
		} 
		else {
			newWidth = maxHeight * imageRatio;
			newHeight = maxHeight;
		}

		transform.localScale.x = newWidth;
		transform.localScale.z = newHeight;

	}
}
