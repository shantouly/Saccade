using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RandomPointGenerator : MonoBehaviour
{
    public GameObject pointPrefab; // ���Ԥ����
    public GameObject initialPointPrefab; // ��ʼ�ĵ�
    private GameObject currentPoint;
    private float spawnInterval = 2f; // �趨ÿ������ʱ���������һ�������
    private float minX = 280f; // X����Сֵ
    private float maxX = 800f; // X�����ֵ
    private float minY = 128f;
    private float maxY = 355f;
    private bool isShow = true;
    int z = -380;
    //int x = -250;
    //float y = 280;

    Vector3 randomPosition;

    // ����Ч��
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed;
    public GameObject panel;

    // ��ȡ���ĵ���¼�
    private Ray ray;
    private RaycastHit hit;
    Vector3 target;


    private void Update()
    {
        // �����¼��̿ո�������Կ�ʼ
        if(Input.GetKeyDown(KeyCode.Space))
        {
            HideInitialPrefab();
            InvokeRepeating("GenerateRandomPoints", 0f, spawnInterval);
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(ShowPanel(panel));
        }else if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(HidePanel(panel));
        }

        MousePosition();

        isEqual();
    }

    void GenerateRandomPoints()
    {
        // ���֮ǰ��������㣬������
        if(currentPoint != null)
        {
            Destroy(currentPoint);
        }

        // ����������λ��
        randomPosition =  new Vector3(Random.Range(minX,maxX), Random.Range(minY, maxY),z);

        // ���������
        currentPoint = Instantiate(pointPrefab,randomPosition,Quaternion.identity);
    }

    void HideInitialPrefab()
    {
        Destroy(initialPointPrefab);
        isShow = false; 
    }

    void MousePosition()
    {
        // �������Ļ��λ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //���Ƴ�һ�����������ĺ�ɫ����
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
        }
        target = hit.point;//��ȡ��������λ��

        Debug.Log("��ȡ������������λ��:" + target);
    }

    void isEqual()
    {
        if(target == initialPointPrefab.transform.position)
        {
            HideInitialPrefab();
        }
    }

    IEnumerator ShowPanel(GameObject panel)
    {
        float timer = 0;
        while (timer <= 1)
        {
            panel.transform.localScale = Vector3.one * showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    IEnumerator HidePanel(GameObject panel)
    {
        float timer = 0;
        while (timer <= 1)
        {
            panel.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }
}




