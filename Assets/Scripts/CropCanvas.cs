﻿using System;
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
    bool canDrag = false;
    bool starting = false;
    public void StartCrop()
    {
        tex = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;

        if (UpperCube != null) Destroy(UpperCube);
        if (LowerCube != null) Destroy(LowerCube);

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
        /*
        Vector3 upperDirect = (UpperCube.transform.position - r.origin).normalized;
        Vector3 lowerDirect = (LowerCube.transform.position - r.origin).normalized;
        Vector3 aimDirection = r.direction.normalized;

        Debug.DrawRay(r.origin, upperDirect, Color.red,2);
        Debug.DrawRay(r.origin, r.direction, Color.white,2);
        Debug.DrawRay(r.origin, lowerDirect, Color.blue,2);
        */

        starting = true;
    }

    public void Dragging(Ray r)
    {
        if (starting)
        {
            starting = false;
            RaycastHit hit2;

            LayerMask mask2 = LayerMask.GetMask("EditCanvas");
            Debug.DrawRay(r.origin, r.direction * 100, Color.green, 3);
            if (Physics.Raycast(r.origin, r.direction, out hit2, mask2))
            {
                Debug.Log("drag start hit: " + hit2.collider.gameObject.name);
                if (hit2.collider.gameObject.CompareTag("Canvas"))
                {

                    Debug.Log("hit coords: " + hit2.textureCoord.ToString("F3"));
                    float xloc = PixelToLocX(hit2.textureCoord.x * tex.width);
                    float yloc = PixelToLocY(hit2.textureCoord.y * tex.height);

                    Vector3 dragStartLoc = new Vector3(xloc, 0, yloc);

                    Debug.LogFormat("drag start: " + dragStartLoc.ToString("F3"));
                    Debug.LogFormat("upper: " + UpperCube.transform.localPosition.ToString("F3"));
                    Debug.LogFormat("lower: " + LowerCube.transform.localPosition.ToString("F3"));


                    Debug.LogFormat("upperdist: {0}, lowerdist:{1}", (UpperCube.transform.localPosition - dragStartLoc).magnitude, (LowerCube.transform.localPosition - dragStartLoc).magnitude);

                    upperCubeDragging = (UpperCube.transform.localPosition - dragStartLoc).magnitude < (LowerCube.transform.localPosition - dragStartLoc).magnitude;
                    canDrag = true;
                }
            }
            else
            {
                Debug.Log("no hit");
                canDrag = false;
            }

            //Debug.LogFormat("upperdist: {0}, lowerdist: {1}", (upperDirect - aimDirection).magnitude, (lowerDirect - aimDirection).magnitude);

            //upperCubeDragging = (upperDirect - aimDirection).magnitude < (lowerDirect - aimDirection).magnitude;
            Debug.LogFormat("started dragging crop, upper:{0}", upperCubeDragging);
        }


        if (!canDrag)
        {
            return;
        }
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("EditCanvas");
        if (Physics.Raycast(r.origin, r.direction, out hit, mask))
        {
            Vector2 hitcoord = hit.textureCoord;
            //Debug.Log("hit: " + hit.collider.gameObject.name);

            //Debug.LogFormat("dragging at {0},{1}", hitcoord.x, hitcoord.y);
            GameObject cube = (upperCubeDragging) ? UpperCube : LowerCube;
            cube.transform.localPosition = new Vector3(PixelToLocX(hitcoord.x * tex.width), 0, PixelToLocY(hitcoord.y * tex.height));
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

        int upperx = LocToPixelX(UpperCube.transform.localPosition.x);
        int lowerx = LocToPixelX(LowerCube.transform.localPosition.x);
        int uppery = LocToPixelY(UpperCube.transform.localPosition.z);
        int lowery = LocToPixelY(LowerCube.transform.localPosition.z);

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
