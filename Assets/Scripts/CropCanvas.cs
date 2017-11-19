using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropCanvas : MonoBehaviour
{
    GameObject cube1;
    GameObject cube2;

    public GameObject Line;

    Vector2 coords1;
    Vector2 coords2;

    //bool upperCubeDragging;
    public GameObject CubePrefab;
    bool cropping = false;
    bool useCube1 = false;

    CanvasSizer cs;

    public void Start()
    {
        cs = GetComponent<CanvasSizer>();
        Line.SetActive(false);
    }

    public void LateUpdate()
    {
        if (cropping)
        {
            //Debug.Log("lateupdate cropcanvas");
            cube1.transform.position = cs.GetWorldPosition(coords1);
            cube2.transform.position = cs.GetWorldPosition(coords2);

            cube1.transform.localRotation = transform.parent.localRotation;
            cube2.transform.localRotation = transform.parent.localRotation;
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
        Line.SetActive(true);

        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("EditCanvas");
        //Debug.DrawRay(r.origin, r.direction * 100, Color.green, 3);
        if (Physics.Raycast(r.origin, r.direction, out hit, mask))
        {
            //hit occurred
            Texture2D image = cs.GetImage();
            Vector2 textureCoord = Vector2.Scale(hit.textureCoord, new Vector2(image.width, image.height));
            useCube1 = Cube1Closer(textureCoord);
            Debug.Log((useCube1) ? "using cube1" : "using cube2");
        }
    }

    public void Apply()
    {
        Debug.Log("applying crop");
        CropRect cropRect = GetCropRect();

        Texture2D image = cs.GetImage();
        Color[] pixel = image.GetPixels(Mathf.RoundToInt(cropRect.x), cropRect.y, cropRect.width, cropRect.height, 0);

        Texture2D newtex = new Texture2D(cropRect.width, cropRect.height);
        newtex.SetPixels(pixel);
        newtex.Apply();

        Debug.Log("here");
        cs.SetImage(newtex);
        Cancel();
        Debug.Log("done applying");
    }

    CropRect GetCropRect()
    {
        CropRect res = new CropRect();
        Texture2D image = cs.GetImage();

        int x1 = Mathf.Clamp(Mathf.RoundToInt(coords1.x), 0, image.width - 1);
        int x2 = Mathf.Clamp(Mathf.RoundToInt(coords2.x), 0, image.width - 1);
        int y1 = Mathf.Clamp(Mathf.RoundToInt(coords1.y), 0, image.height - 1);
        int y2 = Mathf.Clamp(Mathf.RoundToInt(coords2.y), 0, image.height - 1);

        res.x = Mathf.Min(x1, x2);
        res.y = Mathf.Min(y1, y2);

        res.width = Mathf.Abs(x1 - x2 + 1);
        res.height = Mathf.Abs(y1 - y2 + 1);

        return res;
    }


    public void Dragging(Ray r)
    {
        if (cropping)
        {
            RaycastHit hit;

            LayerMask mask = LayerMask.GetMask("EditCanvas");
            //Debug.DrawRay(r.origin, r.direction * 100, Color.green, 3);
            if (Physics.Raycast(r.origin, r.direction, out hit, mask))
            {
                //hit occurred
                Texture2D image = cs.GetImage();
                Vector2 textureCoord = Vector2.Scale(hit.textureCoord, new Vector2(image.width, image.height));

                //GameObject cube = (useCube1) ? cube1 : cube2;
                if (useCube1)
                {
                    coords1 = textureCoord;
                }
                else
                {
                    coords2 = textureCoord;
                }

                CropRect cropRect = GetCropRect();
                Line.transform.localPosition = new Vector3((float)cropRect.x / image.width * -10 + 5, 0.01f, (float)cropRect.y / image.height * -10 + 5);
                Line.transform.localScale = new Vector3(-(float)cropRect.width / image.width * 10, 0, -(float)cropRect.height / image.height * 10);
            }
        }
    }

    bool Cube1Closer(Vector2 textureCoord)
    {
        return (textureCoord - coords1).magnitude < (textureCoord - coords2).magnitude;
    }

    public void Cancel()
    {
        DestroyCubes();
        cropping = false;
        Line.SetActive(false);
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

    struct CropRect
    {
        public int x;
        public int y;
        public int width;
        public int height;
    }
}
