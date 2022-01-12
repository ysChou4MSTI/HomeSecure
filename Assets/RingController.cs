using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RingController : MonoBehaviour
{
    [HideInInspector]
    public string[] ring_names;
    [HideInInspector]
    public int current_ring;
    [HideInInspector]
    public float current_volume; // sound volume

    private AudioSource source;

    private AudioClip[] rings;

    void Start()
    {
        current_ring = 0;
        current_volume = 50;
        ring_names = new string[3];
        ring_names[0] = "铃声1";
        ring_names[1] = "铃声2";
        ring_names[2] = "铃声3";
        source = GameObject.Find("LoggerBase").GetComponent<AudioSource>();
        rings = new AudioClip[3];
        rings[0] = Resources.Load<AudioClip>("ring1");
        rings[1] = Resources.Load<AudioClip>("ring2");
        rings[2] = Resources.Load<AudioClip>("ring3");
    }

    void Update()
    {
        switch (current_ring)
        {
            case 0:
                source.clip = rings[0];
                break;
            case 1:
                source.clip = rings[1];
                break;
            case 2:
                source.clip = rings[2];
                break;
            default:
                break;
        }
        source.volume = current_volume / 100f;
    }

    public void RingStart()
    {
        source.Play();
    }
    public void RingStop()
    {
        source.Stop();
    }
}
