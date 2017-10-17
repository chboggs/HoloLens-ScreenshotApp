using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropCanvas : MonoBehaviour
{
    public float Canvasdimx = 5;
    public float Canvasdimy = 5;
    Texture2D tex;
    public GameObject UpperCube;
    public GameObject LowerCube;

    bool upperCubeDragging;
    public GameObject CubePrefab;
    // Use this for initialization



    public void MakeCubes()
    {
        tex = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        UpperCube = Instantiate(CubePrefab);
        LowerCube = Instantiate(CubePrefab);
        UpperCube.transform.parent = transform;
        LowerCube.transform.parent = transform;
        UpperCube.transform.localRotation = Quaternion.identity;
        LowerCube.transform.localRotation = Quaternion.identity;
        UpperCube.transform.localPosition = new Vector3(Canvasdimx, 0, -Canvasdimy);
        LowerCube.transform.localPosition = new Vector3(-Canvasdimx, 0, Canvasdimy);
        //UpperCube.GetComponent<GazeReceiver>().DragEvent.AddListener(Dragging);
        //LowerCube.GetComponent<GazeReceiver>().DragEvent.AddListener(Dragging);

    }

    public void DestroyCubes()
    {
        Destroy(UpperCube);
        Destroy(LowerCube);
    }

    public void StartDrag(Ray r)
    {
        Vector3 upperDirect = (UpperCube.transform.position - r.origin).normalized;
        Vector3 lowerDirect = (LowerCube.transform.position - r.origin).normalized;
        Vector3 aimDirection = r.direction.normalized;

        Debug.LogFormat("upperdist: {0}, lowerdist: {1}", (upperDirect - aimDirection).magnitude, (lowerDirect - aimDirection).magnitude);

        upperCubeDragging = (upperDirect - aimDirection).magnitude < (lowerDirect - aimDirection).magnitude;
        Debug.LogFormat("started dragging crop, upper:{0}", upperCubeDragging);
    }

    public void Dragging(Ray r)
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("EditCanvas");
        if (Physics.Raycast(r.origin, r.direction, out hit, mask))
        {
            Vector2 hitcoord = hit.textureCoord;
            //Debug.Log("hit: " + hit.collider.gameObject.name);

            //Debug.LogFormat("dragging at {0},{1}", hitcoord.x, hitcoord.y);
            GameObject cube = (upperCubeDragging) ? UpperCube : LowerCube;
            cube.transform.localPosition = new Vector3(PixelXToLocX(hitcoord.x * tex.width), 0, PixelYToLocY(hitcoord.y * tex.height));
        }
    }

    public void Cancel()
    {
        DestroyCubes();
    }

    public void Apply()
    {
        Debug.LogFormat("{0},{1}", UpperCube.transform.localPosition.x, UpperCube.transform.localPosition.z);
        Debug.Log("apply");

        int upperx = Mathf.FloorToInt(((5 - UpperCube.transform.localPosition.x) / 10) * tex.width);
        int lowerx = Mathf.FloorToInt(((5 - LowerCube.transform.localPosition.x) / 10) * tex.width);
        int uppery = Mathf.FloorToInt(((5 - UpperCube.transform.localPosition.z) / 10) * tex.height);
        int lowery = Mathf.FloorToInt(((5 - LowerCube.transform.localPosition.z) / 10) * tex.height);

        Debug.Log("1");

        //int newheight = (uppery - lowery) * tex.height;
        //int newwidth = (upperx - lowerx) * tex.width;

        int minx = Mathf.Min(upperx, lowerx);
        int miny = Mathf.Min(uppery, lowery);
        Debug.Log("2");

        int newwidth = Mathf.Abs(upperx - lowerx);
        int newheight = Mathf.Abs(uppery - lowery);

        Debug.LogFormat("min: {0},{1}, dim:{2},{3}", minx, miny, newwidth, newheight);


        Color[] pixel = tex.GetPixels(minx, miny, newwidth, newheight, 0);

        Texture2D newtex = new Texture2D(newwidth, newheight);
        newtex.SetPixels(pixel);
        newtex.Apply();
        GetComponent<MeshRenderer>().material.mainTexture = newtex;
        Cancel();
    }

    int XLocToPixelX(float x)
    {
        return Mathf.FloorToInt(((Canvasdimx - x) / (Canvasdimx * 2)) * tex.width);
    }
    int YLocToPixelY(float y)
    {
        return Mathf.FloorToInt(((Canvasdimy - y) / (Canvasdimy * 2)) * tex.height);
    }

    float PixelXToLocX(float x)
    {
        return -(x / tex.width * (Canvasdimx * 2) - Canvasdimx);
    }

    float PixelYToLocY(float y)
    {
        return -(y / tex.height * (Canvasdimy * 2) - Canvasdimy);
    }

}
