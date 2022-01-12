using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CallController : MonoBehaviour
{
    [HideInInspector]
    public string[] call_names;
    [HideInInspector]
    public int current_call;
    [HideInInspector]
    public float current_volume; // sound volume

    private AudioSource source;

    private AudioClip[] calls;

    void Start()
    {
        current_call = 0;
        current_volume = 50;
        call_names = new string[2];
        call_names[0] = "呼叫管理中心";
        call_names[1] = "呼叫技术中心";
        source = GameObject.Find("TriPanelBase").GetComponent<AudioSource>();
        calls = new AudioClip[2];
        calls[0] = Resources.Load<AudioClip>("call1");
        calls[1] = Resources.Load<AudioClip>("call2");
    }

    void Update()
    {
        switch (current_call)
        {
            case 0:
                source.clip = calls[0];
                break;
            case 1:
                source.clip = calls[1];
                break;
            default:
                break;
        }
        source.volume = current_volume / 100f;
    }

    public void CallStart()
    {
        source.Play();
    }
    public void CallStop()
    {
        source.Stop();
    }
}