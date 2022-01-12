using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class LightController : MonoBehaviour
{
    public MonitorSystem monitor_system;
    private Light[] inside_lights;
    private Light[] outside_lights;
    [HideInInspector]
    public bool smart_light_trigger; // if used, then inside_light is controlled by person's position
    [HideInInspector]
    public bool inside_light_on;
    [HideInInspector]
    public bool outside_light_on;

    void Start()
    {
        inside_lights = new Light[11];
        outside_lights = new Light[26];
        for (int i = 0; i < 11; i++)
        {
            inside_lights[i] = GameObject.Find("Point light" + (i + 1).ToString()).GetComponent<Light>();
        }
        for (int i = 0; i < 26; i++)
        {
            outside_lights[i] = GameObject.Find("light" + (i + 1).ToString()).GetComponent<Light>();
        }
        smart_light_trigger = true;
        inside_light_on = true;
        outside_light_on = true;
    }

    void Update()
    {
        bool inside_on = smart_light_trigger ? monitor_system.person_inside : inside_light_on;
        if (inside_on)
        {
            for (int i = 0; i < 11; i++)
            {
                inside_lights[i].intensity = 1.2f;
            }
        }
        else
        {
            for (int i = 0; i < 11; i++)
            {
                inside_lights[i].intensity = 0;
            }
        }
        if (outside_light_on)
        {
            for (int i = 0; i < 26; i++)
            {
                outside_lights[i].intensity = 1.5f;
            }
        }
        else
        {
            for (int i = 0; i < 26; i++)
            {
                outside_lights[i].intensity = 0;
            }
        }
        if (monitor_system.is_alert)
        { // alert, use red light
            for (int i = 0; i < 11; i++)
            {
                inside_lights[i].color = Color.red;
            }
            for (int i = 0; i < 26; i++)
            {
                outside_lights[i].color = Color.red;
            }
        }
        else
        {
            for (int i = 0; i < 11; i++)
            {
                inside_lights[i].color = Color.white;
            }
            for (int i = 0; i < 26; i++)
            {
                outside_lights[i].color = Color.white;
            }
        }
    }
}
