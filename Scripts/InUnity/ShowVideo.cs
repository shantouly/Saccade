using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShowVideo : MonoBehaviour
{
    public RawImage rawImage;
    private Connect connect;
    Texture2D texture;

    int width = 640;
    int height = 480;

    private void Start()
    {
        texture = new Texture2D(width, height);
        connect = GetComponent<Connect>();
    }

    private void Update()
    {
        // ����ͼ�����ݴ�С
        byte[] sizeInfo = new byte[4];
        int bytesRead = 0;
        while (bytesRead < sizeInfo.Length)
        {
            bytesRead += connect.stream.Read(sizeInfo, bytesRead, sizeInfo.Length - bytesRead);
        }
        int size = BitConverter.ToInt32(sizeInfo, 0);

        // ����ͼ������
        byte[] imageData = new byte[size];
        bytesRead = 0;
        while (bytesRead < size)
        {
            bytesRead += connect.stream.Read(imageData, bytesRead, size - bytesRead);
        }

        // �����µ�Texture2D����
        texture = new Texture2D(width, height);
        texture.LoadImage(imageData);   // ����imageData��JPEG��ʽ��ͼ������
        texture.Apply();

        // ��ͼ������Ӧ����RawImage
        rawImage.texture = texture;
    }

    
}
