using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator myDoor;

    [SerializeField] private string doorOpen = "DoorOpen";
    [SerializeField] private string doorClose = "DoorClose";

    void Start()
    {
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void Open(){
        myDoor.Play(doorOpen, 0, 0.0f);
    }

    public void Close(){
        myDoor.Play(doorClose, 0, 0.0f);
    }
}
