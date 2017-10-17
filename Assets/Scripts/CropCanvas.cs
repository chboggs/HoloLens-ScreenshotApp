using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropCanvas : MonoBehaviour
{
    public float x = 5;
    public float y = 5;
    Texture2D tex;
    public GameObject UpperCube;
    public GameObject LowerCube;

    bool upperCubeDragging;
    public GameObject CubePrefab;
    // Use this for initialization
    void Start()
    {

    }

    public void MakeCubes()
    {
        UpperCube = Instantiate(CubePrefab);
        LowerCube = Instantiate(CubePrefab);
        UpperCube.transform.parent = transform;
        LowerCube.transform.parent = transform;
        UpperCube.transform.localPosition = new Vector3(x, 0, -y);
        LowerCube.transform.localPosition = new Vector3(-x, 0, y);
        UpperCube.GetComponent<GazeReceiver>().DragEvent.AddListener(Dragging);
        LowerCube.GetComponent<GazeReceiver>().DragEvent.AddListener(Dragging);
    }

    public void StartDrag(Ray r)
    {
        Vector3 upperDirect = (UpperCube.transform.position - r.origin).normalized;
        Vector3 lowerDirect = (LowerCube.transform.position - r.origin).normalized;
        Vector3 aimDirection = r.direction.normalized;

        upperCubeDragging = (upperDirect - aimDirection).magnitude < (lowerDirect - aimDirection).magnitude;
    }

    public void Dragging(Ray r)
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("EditCanvas");
        if (Physics.Raycast(r.origin, r.direction, out hit, mask))
        {
            Vector2 hitcoord = hit.textureCoord;
            GameObject cube = (upperCubeDragging) ? UpperCube : LowerCube;
            cube.transform.localPosition = new Vector3((float)hitcoord.x / tex.width * 2 * x - x, 0, (float)hitcoord.y / tex.height * 2 * y - y);
        }
    }
}
