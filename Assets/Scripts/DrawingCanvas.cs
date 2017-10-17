using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCanvas : MonoBehaviour {
    public Texture2D tex;
	// Use this for initialization
	void Start () {
        tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
	}
	
    public void Draw(Ray r)
    {
        if(tex == null)tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
        //Debug.Log("got drag");
        RaycastHit hit;
        if(Physics.Raycast(r.origin, r.direction, out hit))
        {
            Vector2 hitcoord = hit.textureCoord;
            //Debug.Log("drawing at: "+hitcoord.ToString());
            //Debug.Log("tex is null: " + (tex == null).ToString());
            //  tex.SetPixel(, Mathf.FloorToInt(hitcoord.y * tex.height), Color.red);
            int x = Mathf.FloorToInt(hitcoord.x * tex.width);
            int y = Mathf.FloorToInt(hitcoord.y * tex.height);
            for (int i = -15; i < 15; i++)
            {
                for(int j = -15; j < 15; j++)
                {
                    tex.SetPixel(i+x, j+y, Color.red);
                }
            }
            tex.Apply();
        }
    }
}
