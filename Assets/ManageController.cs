using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManageController : MonoBehaviour
{

    public MonitorSystem monitor_system;
    [HideInInspector]
    public int manager_id;
    [HideInInspector]
    public string[] names;

    void Start()
    {
        names = new string[3];
        names[0] = "张三";
        names[1] = "李四";
        names[2] = "王五";
        manager_id = 0;
    }

    void Update()
    {
    }

}
