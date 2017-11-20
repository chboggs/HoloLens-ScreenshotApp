using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flasher : MonoBehaviour
{
    public float FlashTime = 1;
    bool flash = false;

    public void Flash()
    {
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        if (flash)
        {
            yield break;
        }
        flash = true;
        Debug.Log("flashing");
        Image r = GetComponent<Image>();
        float start = Time.time;

        while (start + FlashTime > Time.time)
        {
            float t = (Time.time - start) / FlashTime;
            float alpha = Mathf.Sin(1.3f* (Mathf.Log(t + 0.1f,2.71828f) + 2.3f));
            r.color = new Color(1, 1, 1, alpha);

            yield return null;
        }
        r.color = new Color(1, 1, 1, 0);
        flash = false;
    }
}
