using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AlertController : MonoBehaviour
{
    public MonitorSystem monitor_system;
    private AudioSource source;
    public LightController light_controller;

    [HideInInspector]
    public int alert_count; // increase when button pressed, trigger alert when high enough 
    [HideInInspector]
    public bool alert_enable;

    void Start()
    {
        source = GameObject.Find("OutSidePanel").GetComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("alert");
        alert_count = 0;
        alert_enable = true;
    }

    void Update()
    {
        if (alert_count > 200)
        {
            AlertStart();
            alert_count = 0;
        }
        alert_count = alert_count > 0 ? alert_count - 1 : 0; // decrease every frame
    }

    public void AlertStart()
    {
        monitor_system.is_alert = true;
        monitor_system.status = Status.ALERT;
        light_controller.outside_light_on = true; // turn on all lights
        light_controller.inside_light_on = true;
        source.Play();
    }
    public void AlertStop()
    {
        monitor_system.is_alert = false;
        source.Stop();
    }
}
