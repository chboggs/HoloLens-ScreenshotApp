using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSizer : MonoBehaviour {

    MeshRenderer mr;
	float maxWidth;
	float maxHeight;

	// Use this for initialization
	void Start () {
		mr = GetComponent<MeshRenderer>();
        maxWidth = transform.localScale.x;
        maxHeight = transform.localScale.z;
	}

	public void SetImage(Texture2D image){
		mr.material.mainTexture = image;
        Debug.Log("set image");

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

        transform.localScale = new Vector3(newWidth, 1, newHeight);
	}

    public Texture2D GetImage()
    {
        return mr.material.mainTexture as Texture2D;
    }

    public Vector2 GetTextureCoords(Vector3 worldPosition)
    {
        
        Vector3 localPosition = transform.InverseTransformVector(worldPosition);
        Texture2D image = GetImage();
        return new Vector2((localPosition.x*0.1f+0.5f) * image.width, (localPosition.z*0.5f+0.5f) * image.height);
    }

    public Vector3 GetWorldPosition(Vector2 textureCoords)
    {
        Texture2D image = GetImage();
        Vector3 localPosition = new Vector3((textureCoords.x/image.width - 0.5f)*10, 0, (textureCoords.y/image.height - 0.5f)*10);
        return transform.TransformPoint(localPosition);
    }
    
}
