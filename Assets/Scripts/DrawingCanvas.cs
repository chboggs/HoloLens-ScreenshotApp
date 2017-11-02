using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCanvas : MonoBehaviour
{
    public Texture2D tex;
    // Use this for initialization

	public float Canvasdimx = 5;
	public float Canvasdimy = 5;

    int px = -100;
    int py = -100;

    public int Radius = 10;
	public Color DrawColor = Color.red;
    public int MaxDistanceToConnect = 20;
    public float NumDrawsInConnect = 4;

	Color[] data;


	public GameObject LinePrefab;
	LineRenderer lr;

    private void OnEnable()
    {
        StartDraw();
    }

    public void StartDraw()
    {
        Debug.Log("StartDraw");
        tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
        px = -100;
        py = -100;
		//data = tex.GetPixels ();
		lr = Instantiate (LinePrefab).GetComponent<LineRenderer> ();
		lr.gameObject.transform.parent = transform;
		lr.startColor = DrawColor;
		lr.endColor = DrawColor;
    }

	public void Draw(Ray r){
		Debug.Log("got drag in draw");
		RaycastHit hit;
		if (Physics.Raycast(r.origin, r.direction, out hit))
		{
			//append hit loc
			Vector3 hitloc = transform.InverseTransformPoint (hit.point);
			lr.SetPosition (lr.positionCount, hitloc);
		}
	}

    public void DrawOld(Ray r)
    {
        Debug.Log("dragdrag");
		if (tex == null)
		{
			StartDraw ();
		}
        Debug.Log("got drag in draw");
        RaycastHit hit;
        if (Physics.Raycast(r.origin, r.direction, out hit))
        {
            Vector2 hitcoord = hit.textureCoord;
            //Debug.Log("drawing at: "+hitcoord.ToString());
            //Debug.Log("tex is null: " + (tex == null).ToString());
            //  tex.SetPixel(, Mathf.FloorToInt(hitcoord.y * tex.height), Color.red);
            int x = Mathf.FloorToInt(hitcoord.x * tex.width);
            int y = Mathf.FloorToInt(hitcoord.y * tex.height);
			            
            if (new Vector2(x - px, y - py).magnitude < MaxDistanceToConnect)
            {
                //fill 
                Vector2 s = new Vector2(px, py);
                Vector2 d = new Vector2(x, y);
                for (int i = 1; i <=NumDrawsInConnect; i++)
                {
                    Vector2 l = Vector2.Lerp(s, d, (float)i / NumDrawsInConnect);
                    DrawCircle(Mathf.FloorToInt(l.x), Mathf.FloorToInt(l.y));
                }
            }
            else
            {
                DrawCircle(x, y);
            }

            px = x;
            py = y;

            tex.SetPixels(data);
            tex.Apply();
        }
    }

	public void Apply(){

		data = tex.GetPixels ();

		int px = -100;
		int py = -100;

		Vector3[] vs = new Vector3[lr.positionCount];
		lr.GetPositions (vs);
		foreach( Vector3 v in vs){
			int x  = LocToPixelX(v.x);
			int y = LocToPixelY(v.z);

			if (new Vector2(x - px, y - py).magnitude < MaxDistanceToConnect)
			{
				//fill 
				Vector2 s = new Vector2(px, py);
				Vector2 d = new Vector2(x, y);
				for (int i = 1; i <=NumDrawsInConnect; i++)
				{
					Vector2 l = Vector2.Lerp(s, d, (float)i / NumDrawsInConnect);
					DrawCircle(Mathf.FloorToInt(l.x), Mathf.FloorToInt(l.y));
				}
			}
			else
			{
				DrawCircle(x, y);
			}


			px = x;
			py = y;
		}
		tex.SetPixels (data);
		tex.Apply ();
	}

	void DrawCircle(int x, int y)
    {
        for (int i = -Radius; i <= Radius; i++)
        {
            int other = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(Radius, 2) - Mathf.Pow(i,2)));
            for (int j = -other; j <= other; j++)
            {
                int positionX = x + i;
                int positionY = y + j;
                if (positionX >= 0 && positionX < tex.width && positionY >= 0 && positionY < tex.height && new Vector2(i, j).magnitude <= Radius)
                {
					data[GetLinearCoordinate(positionX, positionY)] = DrawColor;
                }
            }
        }
    }

	public void SetRadius(int r){
		Radius = r;
	}

	public void SetSmall(){
		SetRadius(5);
	}

	public void SetMedium(){
		SetRadius (10);
	}

	public void SetLarge(){
		SetRadius (20);
	}

	public void SetColor(Color c){
		DrawColor = c;
	}

	public void SetRed(){
		SetColor (Color.red);
	}

	public void SetBlue(){
		SetColor (Color.blue);
	}

	public void SetGreen(){
		SetColor (Color.green);
	}

	public void SetWhite(){
		SetColor (Color.white);
	}

	public void SetBlack(){
		SetColor (Color.black);
	}		

    int GetLinearCoordinate(int x, int y)
    {
        return x + y * tex.width;
    }

	int LocToPixelX(float x)
	{
		return Mathf.FloorToInt(((Canvasdimx - x) / (Canvasdimx * 2)) * tex.width);
	}
	int LocToPixelY(float y)
	{
		return Mathf.FloorToInt(((Canvasdimy - y) / (Canvasdimy * 2)) * tex.height);
	}

	float PixelToLocX(float x)
	{
		return -(x / tex.width * (Canvasdimx * 2) - Canvasdimx);
	}

	float PixelToLocY(float y)
	{
		return -(y / tex.height * (Canvasdimy * 2) - Canvasdimy);
	}
}
