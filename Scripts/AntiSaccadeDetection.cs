using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class AntiSaccadeDetection : MonoBehaviour
{
    private StreamWriter _writer;
    public string csvFilePath;
    [HideInInspector]
    public bool isStartAntiSaccade = false;
    [HideInInspector]
    public bool isFinish = false;

    public GameObject PointPrefab;
    private GameObject currentPoint;

    private RaycastHit hit;
    private Vector3 target = Vector3.zero;
    private bool isTrue = false;
    public float minX = 10f;
    public float maxX = 1240f;
    public float minY = 10f;
    public float maxY = 540f;
    private float angleDeg;
    private bool isAntiSaccade = false;

    Vector3 randomPosition = new Vector3(670f,275f,0);



    private void Update()
    {
        if (isStartAntiSaccade)
        {
            StartAntiSaccadeDetection();
        }
        if (isFinish)
        {
            CancelInvoke("GenerateRandomPointsOfAnti");
            OnApplicationQuit();
            if (currentPoint != null)
            {
                Destroy(currentPoint);
            }
            isFinish = false;
        }
    }

    // ����prosaccade���
    void StartAntiSaccadeDetection()
    {
        if(currentPoint != null)
        {
            CheckMouseAngle();
        }      
        if(isStartAntiSaccade && _writer != null)
        {
            // ��������
            SaveAntiSaccadeDataToCSV(Time.time, target.x, target.y,randomPosition.x,randomPosition.y,angleDeg,isAntiSaccade);
        }
    }

    // ���prosaccade��״̬�µ�λ�ù�ϵ
    void CheckMouseAngle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //���Ƴ�һ�����������ĺ�ɫ����
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
        }
        Vector3 vectorLine = randomPosition - new Vector3(randomPosition.x - 200, randomPosition.y, randomPosition.z);
        Vector3 mouseTocuttenPoint = currentPoint.transform.position - hit.point;

        // ��������ֱ�ߵĵ��
        float dotProduct = Vector3.Dot(vectorLine.normalized, mouseTocuttenPoint.normalized);

        // ʹ�����ҹ�ʽ����нǣ����ȣ�
        float angleRad = Mathf.Acos(dotProduct);

        // ������ת��Ϊ�Ƕ�
        angleDeg = angleRad * Mathf.Rad2Deg;

        // �жϽǶ��Ƿ����жϵķ�Χ��
        if(angleDeg < 25 || angleDeg > 155)
        {
            isTrue = true;
            isAntiSaccade = true;
        }
        else
        {
            isTrue = false;
            isAntiSaccade = false;
        }
        Debug.Log("�Ƕ�Ϊ��" + angleDeg + "�Ƿ���ȷ" + isTrue);
    }
    public void startAntiSaccade()
    {
        startSaveData();
        InvokeRepeating("GenerateRandomPointsOfAnti", 0f, 3f);
    }

    void GenerateRandomPointsOfAnti()
    {
        // ���֮ǰ��������㣬������
        if (currentPoint != null)
        {
            Destroy(currentPoint);
        }
        // ������һ��������λ����ΪԲ��
        Vector3 circleCenter = randomPosition;

        // ����Ƕ�
        float angle = Random.Range(0, 360f);

        // ������һ��������λ��

        float newX = circleCenter.x + 120f * Mathf.Cos(angle * Mathf.Deg2Rad);
        float newY = circleCenter.y + 120f * Mathf.Sin(angle * Mathf.Deg2Rad);

        while (newX < minX || newX > maxX || newY < minY || newY > maxY)
        {
            angle = Random.Range(0, 360f);
            newX = circleCenter.x + 120f * Mathf.Cos(angle * Mathf.Deg2Rad);
            newY = circleCenter.y + 120f * Mathf.Sin(angle * Mathf.Deg2Rad);
        }

        // ����������λ��
        randomPosition = new Vector3(newX, newY, 0);

        // ���������
        currentPoint = Instantiate(PointPrefab, randomPosition, Quaternion.identity);
    }


    // ����CSV�ļ���д���ͷ
    public void startSaveData()
    {
        _writer = new StreamWriter(csvFilePath, false, Encoding.UTF8);
        _writer.WriteLine("Time,MouseX,MouseY,DotX,DotY,Angle,isAntiSaccade");
    }

    // ������д���ļ�
    public void SaveAntiSaccadeDataToCSV(float time,float MouseX, float MouseY, float DotX, float DotY,float angle, bool isAntiSaccadeT)
    {
        _writer.WriteLine(time + "," + MouseX + "," + MouseY + "," + DotX + "," + DotY + "," + angle + ","  +isAntiSaccadeT);
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
