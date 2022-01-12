using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;

using PDollarGestureRecognizer;
public class MovementRecognizer : MonoBehaviour
{
    public TextPrinter text_printer;
    public MonitorSystem monitor_system;
    public XRNode inputSource;
    public RingController ring_controller;
    public CallController call_controller;

    public InputHelpers.Button inputButton1;
    public InputHelpers.Button inputButton2;
    public float InputThreshold = 0.1f;
    public GameObject debugCubePrefab;

    public Transform movementSource;

    public float XYPositionThresholdDistance = 0.2f;
    public float newPositionThresholdDistance = 0.1f;

    private bool volume_adjust_enable = false;
    private float volume_yposition;

    Vector3 Startposition = new Vector3();
    private bool isMoving = false;
    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Vector3> positionList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton1, out bool isPressed1, InputThreshold); // A button
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton2, out bool isPressed2, InputThreshold); // B button

        if (!isMoving && isPressed1)
        {
            StartMovement();
        }

        else if (isMoving && !isPressed1)
        {
            EndMovement();
        }

        else if (isMoving && isPressed1)
        {
            UpdateMovement();
        }

        if (!isPressed2)
        {
            volume_adjust_enable = false;
        }
        else if (!volume_adjust_enable)
        {
            volume_adjust_enable = true;
            volume_yposition = movementSource.position.y;
        }
        else if (monitor_system.status == Status.COMMON_RING_SETTING)
        {
            ring_controller.current_volume += (movementSource.position.y - volume_yposition) * 50;
            ring_controller.current_volume = ring_controller.current_volume > 0 ? (ring_controller.current_volume < 100 ? ring_controller.current_volume : 100) : 0;
            volume_yposition = movementSource.position.y;
        }
        else if (monitor_system.status == Status.COMMON_CALL_SETTING)
        {
            call_controller.current_volume += (movementSource.position.y - volume_yposition) * 50;
            call_controller.current_volume = call_controller.current_volume > 0 ? (call_controller.current_volume < 100 ? call_controller.current_volume : 100) : 0;
            volume_yposition = movementSource.position.y;
        }
    }

    void StartMovement()
    {
        isMoving = true;
        //text_printer.Addinfo(movementSource.position.ToString());

        Startposition = movementSource.position;
        positionList.Clear();
        positionList.Add(movementSource.position);
        //if (debugCubePrefab)
        //Destroy(Instantiate(debugCubePrefab, movementSource.position,Quaternion.identity),1);

    }

    void EndMovement()
    {
        isMoving = false;
        Vector2 lastxy = new Vector2(movementSource.position.x, movementSource.position.y);
        Vector2 firstxy = new Vector2(Startposition.x, Startposition.y);
        //text_printer.Addinfo(movementSource.position.ToString());
        if (Vector2.Distance(lastxy, firstxy) < XYPositionThresholdDistance && (movementSource.position.z - Startposition.z) > 0.15f)
        {
            monitor_system.TriggerDoorOpen();
        }
        if (monitor_system.door_open == true && Vector2.Distance(lastxy, firstxy) < XYPositionThresholdDistance && (movementSource.position.z - Startposition.z) < -0.15f)
        {
            monitor_system.TriggerDoorClose();
        }

        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }
        Gesture candidate = new Gesture(pointArray);

        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
        //text_printer.Loginfo(gestureResult.GestureClass + ":" + gestureResult.Score);

        if (gestureResult.Score > 0.8f)
        {
            if (gestureResult.GestureClass == "D")
            {
                monitor_system.DoorOpenButtonPressed();
            }
            else if (gestureResult.GestureClass == "I")
            {
                monitor_system.LightSwitchPressed();
            }
        }
    }

    void UpdateMovement()
    {
        //text_printer.Addinfo("Update Movement");
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            positionList.Add(movementSource.position);
            //if (debugCubePrefab)
            //Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 1);
        }
    }
}
