using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Manager : MonoBehaviour
{
    private ProSaccadeDetection proSaccadeDetection;
    private PanelAnimator panelanimator;
    private AntiSaccadeDetection antiSaccadeDetection;

    private void Start()
    {
        proSaccadeDetection = GetComponent<ProSaccadeDetection>();
        panelanimator = GetComponent<PanelAnimator>();
        antiSaccadeDetection = GetComponent<AntiSaccadeDetection>();
    }

    private void Update()
    {
        // ����proSaccade���
        if(Input.GetKeyDown(KeyCode.N))
        {
            proSaccadeDetection.startProSaccade();
            proSaccadeDetection.isStartProSaccade = true;
        }
        else
        {
            // ����proSaccade���
            if (Input.GetKeyDown(KeyCode.M))
            {
                proSaccadeDetection.isSpawning = true;
                proSaccadeDetection.isStartProSaccade = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // �����������
            StartCoroutine(panelanimator.ShowPanel());
        }
        if(Input.GetKeyDown (KeyCode.S))
        {
            // �����������
            StartCoroutine (panelanimator.HidePanel());
        }
        // ����antiSaccade���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            antiSaccadeDetection.startAntiSaccade();
            antiSaccadeDetection.isStartAntiSaccade = true;
        }
        else
        {
            // ����antisaccade���
            if (Input.GetKeyDown(KeyCode.W))
            {
                antiSaccadeDetection.isFinish = true;
                antiSaccadeDetection.isStartAntiSaccade= false;
            }
        }
    }
}
