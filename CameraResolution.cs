using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 19); // ( ���� / ����)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1) // ���غ��� ���ΰ� �涧
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else // ���غ��� ���ΰ� �涧
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
}
