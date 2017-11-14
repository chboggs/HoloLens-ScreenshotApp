using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropCanvas : MonoBehaviour
{
    //public float Canvasdimx = 5;
    //public float Canvasdimy = 5;

    GameObject cube1;
    GameObject cube2;

    Vector2 coords1;
    Vector2 coords2;

    bool upperCubeDragging;
    public GameObject CubePrefab;
    bool canDrag = false;
    bool starting = false;

    bool cropping = false;

    CanvasSizer cs;

    public void Start()
    {
        cs = GetComponent<CanvasSizer>();
    }

    public void LateUpdate()
    {
        if (cropping)
        {
            //Debug.Log("lateupdate cropcanvas");
            cube1.transform.position = cs.GetWorldPosition(coords1);
            cube1.transform.up = transform.up;
            cube2.transform.position = cs.GetWorldPosition(coords2);
            cube2.transform.up = transform.up;
        }
    }


    public void StartCrop()
    {
        InstantiateCubes();
        Texture2D image = cs.GetImage();
        coords1 = new Vector2(0, image.height - 1);
        coords2 = new Vector2(image.width - 1, 0);
        cropping = true;
    }


    public void StartDrag(Ray r)
    {
        starting = true;
    }

    public void Tap(Ray r)
    {
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("EditCanvas");
        //Debug.DrawRay(r.origin, r.direction * 100, Color.green, 3);
        if (Physics.Raycast(r.origin, r.direction, out hit, mask))
        {
            //hit occurred
            Texture2D image = cs.GetImage();
            Vector2 textureCoord = Vector2.Scale(hit.textureCoord, new Vector2(image.width, image.height));
            bool useCube1 = Cube1Closer(textureCoord);

            //GameObject cube = (useCube1) ? cube1 : cube2;
            Debug.LogFormat("moving cube: {0}", useCube1 ? "cube1" : "cube2");
            if (useCube1)
            {
                coords1 = textureCoord;
            }
            else
            {
                coords2 = textureCoord;
            }
        }
    }

    public void Apply()
    {
        Debug.Log("applying crop");
        Texture2D image = cs.GetImage();

        int x1 = Mathf.Clamp(Mathf.RoundToInt(coords1.x), 0, image.width - 1);
        int x2 = Mathf.Clamp(Mathf.RoundToInt(coords2.x), 0, image.width - 1);
        int y1 = Mathf.Clamp(Mathf.RoundToInt(coords1.y), 0, image.height - 1);
        int y2 = Mathf.Clamp(Mathf.RoundToInt(coords2.y), 0, image.height - 1);

        int minx = Mathf.Min(x1, x2);
        int miny = Mathf.Min(y1, y2);

        int newwidth = Mathf.Abs(x1 - x2 + 1);
        int newheight = Mathf.Abs(y1 - y2 + 1);

        //   Debug.LogFormat("min: {0},{1}, dim:{2},{3}", minx, miny, newwidth, newheight);

        Color[] pixel = image.GetPixels(minx, miny, newwidth, newheight, 0);

        Texture2D newtex = new Texture2D(newwidth, newheight);
        newtex.SetPixels(pixel);
        newtex.Apply();

        Debug.Log("here");
        cs.SetImage(newtex);
        Cancel();
        Debug.Log("done applying");
    }

    /*
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
    */
    bool Cube1Closer(Vector2 textureCoord)
    {
        return (textureCoord - coords1).magnitude < (textureCoord - coords2).magnitude;
    }

    public void Cancel()
    {
        DestroyCubes();
        cropping = false;
    }

    void DestroyCubes()
    {
        if (cube1 != null) Destroy(cube1);
        if (cube2 != null) Destroy(cube2);
    }

    void InstantiateCubes()
    {
        DestroyCubes();
        cube1 = Instantiate(CubePrefab);
        cube2 = Instantiate(CubePrefab);
    }
}
