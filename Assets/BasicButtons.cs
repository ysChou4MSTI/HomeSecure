using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BasicButtons : MonoBehaviour
{
    public MonitorSystem monitor_system;
    [HideInInspector]
    public GameObject door_open;
    [HideInInspector]
    public GameObject door_lock;
    [HideInInspector]
    public GameObject listen;
    [HideInInspector]
    public GameObject call;
    [HideInInspector]
    public GameObject system;
    [HideInInspector]
    public GameObject alert;

    void Start()
    {
        door_open = GameObject.Find("DoorOpenButton");
        door_lock = GameObject.Find("DoorLockButton");
        listen = GameObject.Find("ListenButton");
        call = GameObject.Find("CallButton");
        system = GameObject.Find("SystemButton");
        alert = GameObject.Find("AlertButton");
    }

    void Update()
    {
        if(monitor_system.door_open){
            door_open.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=5;
        }else{
            door_open.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=0;
        }
        if(monitor_system.door_lock){
            door_lock.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=5;
        }else{
            door_lock.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=0;
        }
        if(monitor_system.is_listening){
            listen.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=5;
        }else{
            listen.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=0;
        }
        if(monitor_system.is_calling){
            call.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=5;
        }else{
            call.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=0;
        }
        if(monitor_system.enable){
            system.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=5;
        }else{
            system.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=0;
        }
        if(monitor_system.is_alert){
            alert.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=5;
        }else{
            alert.GetComponentsInChildren<Transform>()[1].GetComponent<Light>().intensity=0;
        }
    }
}
