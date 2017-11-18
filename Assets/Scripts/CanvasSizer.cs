using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSizer : MonoBehaviour
{

    Renderer mr;
    float maxWidth;
    float maxHeight;

    // Use this for initialization
    void Awake()
    {
        mr = GetComponent<Renderer>();
        maxWidth = transform.localScale.x;
        maxHeight = transform.localScale.z;
        //Debug.LogFormat("cs starting, mr is null: {0}", mr == null);
    }

    public void SetImage(Texture2D image)
    {
        //Debug.LogFormat("cs set image: mr is null: {0}", mr == null);
        mr.material.mainTexture = image;

        float canvasRatio = maxWidth / maxHeight;
        float imageRatio = (float)image.width / image.height;

        float newWidth, newHeight;

        if (canvasRatio < imageRatio)
        {
            newWidth = maxWidth;
            newHeight = maxWidth / imageRatio;
        }
        else
        {
            newWidth = maxHeight * imageRatio;
            newHeight = maxHeight;
        }

        transform.localScale = new Vector3(newWidth, 1, newHeight);

        Debug.Log("cs set image done");
    }

    public Texture2D GetImage()
    {
        return mr.material.mainTexture as Texture2D;
    }

    public Vector2 GetTextureCoords(Vector3 worldPosition)
    {

        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        Texture2D image = GetImage();
        return new Vector2((localPosition.x * 0.1f + 0.5f) * image.width, (localPosition.z * 0.5f + 0.5f) * image.height);
    }

    public Vector3 GetWorldPosition(Vector2 textureCoords)
    {

        Texture2D image = GetImage();
        Vector3 localPosition = new Vector3((textureCoords.x / image.width - 0.5f) * -10, 0, (textureCoords.y / image.height - 0.5f) * -10);
        Vector3 worldPosition = transform.TransformPoint(localPosition);
        // Debug.LogFormat("Texturecoords: {0:F2}, world position: {1:F2}", textureCoords, worldPosition);
        return worldPosition;
    }

}
