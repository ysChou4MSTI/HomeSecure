using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelScreen : MonoBehaviour
{
    public MonitorSystem monitor_system;
    // components
    [HideInInspector]
    public GameObject button1;
    [HideInInspector]
    public GameObject button2;
    [HideInInspector]
    public GameObject button3;
    [HideInInspector]
    public GameObject back_button;
    [HideInInspector]
    public TextMeshPro text1;
    [HideInInspector]
    public TextMeshPro text2;
    [HideInInspector]
    public TextMeshPro text3;

    void Start()
    {
        button1 = GameObject.Find("Button1");
        button2 = GameObject.Find("Button2");
        button3 = GameObject.Find("Button3");
        back_button = GameObject.Find("BackButton");
        text1=button1.GetComponentsInChildren<Transform>()[3].GetComponent<TextMeshPro>();
        text2=button2.GetComponentsInChildren<Transform>()[3].GetComponent<TextMeshPro>();
        text3=button3.GetComponentsInChildren<Transform>()[3].GetComponent<TextMeshPro>();
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        back_button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(monitor_system.enable){
            switch(monitor_system.status){
                case Status.IDLE:
                case Status.ALERT:
                case Status.SAFETY_MODE:
                case Status.COMMON_MAIN:
                case Status.MANAGE_MAIN:
                case Status.SAFETY_MAIN:
                case Status.OTHERS_MAIN:
                    back_button.SetActive(false);
                    break;
                default:
                    back_button.SetActive(true);
                    break;
            }
        }else{
            back_button.SetActive(false);
        }
    }

    public void SetButtons(int enables, string t1="",string t2="",string t3=""){
        switch(enables){
            case 0:
                button1.SetActive(false);
                button2.SetActive(false);
                button3.SetActive(false);
                break;
            case 1:
                button1.SetActive(true);
                text1.text=t1;
                button2.SetActive(false);
                button3.SetActive(false);
                break;
            case 2:
                button1.SetActive(true);
                text1.text=t1;
                button2.SetActive(true);
                text2.text=t2;
                button3.SetActive(false);
                break;
            case 3:
                button1.SetActive(true);
                text1.text=t1;
                button2.SetActive(true);
                text2.text=t2;
                button3.SetActive(true);
                text3.text=t3;
                break;    
        }
    }
}
