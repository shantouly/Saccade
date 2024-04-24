using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// ����һ���������prosaccade����
/// </summary>
public class ProSaccadeDetection : MonoBehaviour
{
    private StreamWriter _writer;
    public string csvFilePath;
    [HideInInspector]
    public bool isStartProSaccade = false;
    [HideInInspector]
    public bool isSpawning = false;

    public GameObject DotPrefab;
    private GameObject currentDot;

    private RaycastHit hit;
    private Vector3 target = Vector3.zero;
    private float detectionRadius = 10; // ���ü��뾶
    float distance = 0;
    public float minX = 10f;
    public float maxX = 1240f;
    public float minY = 10f;
    public float maxY = 540f;
    private bool isProSaccade = false;

    // ��ʼ����ʼ��
    Vector3 randomPosition = new Vector3(670f,275f,0f);


    private void Update()
    {
        if (isStartProSaccade)
        {
            StartProSaccadeDetection();  
        }
        if (isSpawning)
        {
            CancelInvoke("GenerateRandomPoints");
            OnApplicationQuit();
            if (currentDot != null)
            {
                Destroy(currentDot);
            }
            isSpawning = false;
        }
    }

    // ����prosaccade���
    void StartProSaccadeDetection()
    {
        CheckMousePosition();
        if (isStartProSaccade && _writer!=null) // ��������� ProSaccade ���
        {
            // Ȼ�󱣴�����
            SaveProSaccadeDataToCSV(Time.time,target.x, target.y, randomPosition.x, randomPosition.y,distance, isProSaccade);
        }
    }

    // ���prosaccade��״̬�µ�λ�ù�ϵ
    void CheckMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //���Ƴ�һ�����������ĺ�ɫ����
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
            target = hit.point;
        }
        if(currentDot != null)
        {
            distance = Vector3.Distance(hit.point, currentDot.transform.position);

            // �жϾ����Ƿ��ڼ��뾶��Χ�ڣ�����ӡ���
            if(distance < detectionRadius)
            {
                Debug.Log("Mouse is near the point: true");
                isProSaccade = true;
            }
            else
            {
                Debug.Log("Mouse is near the point: false");
                isProSaccade = false;
            }
        }
    }

    public void startProSaccade()
    {
        startSaveData();
        InvokeRepeating("GenerateRandomPoints", 0f, 2f);
    }

    void GenerateRandomPoints()
    {
        // ���֮ǰ��������㣬������
        if (currentDot != null)
        {
            Destroy(currentDot);
        }

        // ������һ��������λ����ΪԲ��
        Vector3 circleCenter = randomPosition;

        // ����Ƕ�
        float angle = Random.Range(0, 360f);

        // ������һ��������λ��
        
        float newX = circleCenter.x + 120f * Mathf.Cos(angle * Mathf.Deg2Rad);
        float newY = circleCenter.y + 120f * Mathf.Sin(angle * Mathf.Deg2Rad);

        while(newX < minX || newX > maxX || newY < minY || newY > maxY)
        {
            angle = Random.Range(0, 360f);
            newX = circleCenter.x + 120f * Mathf.Cos(angle * Mathf.Deg2Rad);
            newY = circleCenter.y + 120f * Mathf.Sin(angle * Mathf.Deg2Rad);
        }

        // ����������λ��
        randomPosition = new Vector3(newX,newY,0);

        // ���������
        currentDot = Instantiate(DotPrefab, randomPosition, Quaternion.identity);
    }


    // ����CSV�ļ���д���ͷ
    public void startSaveData()
    {
        _writer = new StreamWriter(csvFilePath, false, Encoding.UTF8);
        _writer.WriteLine("Time,MouseX,MouseY,DotX,DotY,Distance,isProSaccade");
    }

    // ������д���ļ�
    public void SaveProSaccadeDataToCSV(float time,float MouseX, float MouseY, float DotX, float DotY,float Distance, bool isProSaccadeT)
    {
        _writer.WriteLine(time + "," + MouseX + "," + MouseY + "," + DotX + "," + DotY + "," + Distance + "," + isProSaccadeT);
        //Debug.Log("�Ѿ�����");
    }

    public void OnApplicationQuit()
    {
        if (_writer != null)
        {
            _writer.Close();
        }
    }
}
