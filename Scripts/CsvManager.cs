using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class CsvManager : MonoBehaviour
{
    //public string fileName;
    //public List<Book> books = new List<Book>();

    //private void Start()
    //{
    //    // �ļ�·��
    //    string path = Application.streamingAssetsPath + "/" + fileName + ".csv";
    //    // �����ļ����Ƿ����
    //    if (!Directory.Exists(path))
    //    {
    //        Directory.CreateDirectory(Application.streamingAssetsPath);
    //    }
    //    StreamWriter sw = new StreamWriter(path,false,Encoding.UTF8);
    //    sw.WriteLine("id,name,author");

    //    // �洢����
    //    for (int i = 0; i < books.Count; i++)
    //    {
    //        sw.WriteLine($"{books[i].id},{books[i].name},{books[i].author}");
    //    }

    //    sw.Flush();
    //    sw.Close();
    //}

    private StreamWriter csvWriter;
    private string csvFilePath = "Assets/MouseMovementData.csv"; // CSV�ļ�����·��

    private struct MousePositionData
    {
        public float mouseX;
        public float mouseY;

        public MousePositionData(float x, float y)
        {
            mouseX = x;
            mouseY = y;
        }
    }

    void Start()
    {
        // ����CSV�ļ���д���ͷ
        csvWriter = new StreamWriter(csvFilePath,false,Encoding.UTF8);
        csvWriter.WriteLine("Time,X,Y"); // ��ͷ��ʱ�䣬���Xλ�ã����Yλ��
    }

    void Update()
    {
        // ��ȡ��굱ǰλ��
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        // ����һ��MousePositionDataʵ��������д��CSV�ļ�
        MousePositionData data = new MousePositionData(mouseX, mouseY);
        WriteDataToCSV(data);
    }

    void WriteDataToCSV(MousePositionData data)
    {
        // �����λ��д��CSV�ļ�
        csvWriter.WriteLine(Time.time + "," + data.mouseX + "," + data.mouseY);
    }

    void OnApplicationQuit()
    {
        // �ر�CSV�ļ�
        if (csvWriter != null)
        {
            csvWriter.Close();
        }
    }

}
[System.Serializable]
public class Book
{
    public int id;
    public string name;
    public string author;
}

