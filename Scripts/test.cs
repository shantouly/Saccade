using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class test : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Ray ray;
    private RaycastHit hit;
    private bool isTrue = false;

    private void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //���Ƴ�һ�����������ĺ�ɫ����
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
        }
             // ��������AB������BC
            Vector3 vectorAB = pointA.transform.position - pointB.transform.position;
            Vector3 vectorBC = pointA.transform.position - hit.point;

            // ����AB��BC�ĵ��
            float dotProduct = Vector3.Dot(vectorAB.normalized, vectorBC.normalized);

            // ʹ�����ҹ�ʽ����нǣ����ȣ�
            float angleRad = Mathf.Acos(dotProduct);

            // ������ת��Ϊ�Ƕ�
            float angleDeg = angleRad * Mathf.Rad2Deg;
        if(angleDeg < 20)
        {
            isTrue = true;
        }
        else
        {
            isTrue = false;
        }

            Debug.Log("AB��BC�ļнǣ��ȣ���" + angleDeg + "......" + isTrue);
    }
}
