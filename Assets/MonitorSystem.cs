using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    IDLE,
    ALERT,
    SAFETY_MODE,
    COMMON_MAIN,
    COMMON_RING_SETTING,
    COMMON_CALL_SETTING,
    MANAGE_MAIN,
    MANAGE_PASSWORD_CHECK,
    MANAGE_MANAGER_CHOOSE,
    SAFETY_MAIN,
    SAFETY_SETTING,
    SAFETY_PASSWORD_CHECK,
    SAFETY_PASSWORD_OVERRIDE,
    SAFETY_MODE_ENTER_CHECK,
    SAFETY_MODE_EXIT_CHECK,
    OTHERS_MAIN,
};


public class MonitorSystem : MonoBehaviour
{
    // components
    public TextPrinter text_printer;
    public DoorAnim door_anim;
    public PanelScreen panel_screen;
    public SystemButtons system_buttons;
    public BasicButtons basic_buttons;
    public RingController ring_controller;
    public CallController call_controller;
    public LightController light_controller;
    public AlertController alert_controller;
    public MovementRecognizer movement_recognizer;
    public PasswordController password_controller;
    public ManageController manage_controller;

    public string camera_name;
    public string door_name;
    public string light_name;


    // common status
    [HideInInspector]
    public bool enable; // if the system is on
    [HideInInspector]
    public Status status;
    [HideInInspector]
    public bool door_open; // if the door is open
    [HideInInspector]
    public bool door_lock; // if the door is locked
    [HideInInspector]
    public bool bell_ringing; // if the bell is ringing
    [HideInInspector]
    public bool is_listening; // if is listening to other person
    [HideInInspector]
    public bool is_calling; // if is calling
    [HideInInspector]
    public bool is_alert; // if is alert
    [HideInInspector]
    public bool safe_mode; // if is safemode
    [HideInInspector]
    public bool person_inside; // if someone is inside
    private bool person_inside_previous; // previous time's person_inside
    private Vector3 camera_pos; // pos of the person

    // boundaries of the house
    private float house_midx = 3.5f;
    private float house_maxx = 5f;
    private float house_minx = -5f;
    private float house_maxz = 5.4f;
    private float house_midz = -3.15f;
    private float house_minz = -9.6f;

    private GameObject doorlight;
    private Renderer lightRenderer;

    void Start()
    {
        doorlight = GameObject.Find(light_name);
        lightRenderer = doorlight.GetComponent<Renderer>();
        enable = true;
        door_open = false;
        door_lock = false;
        bell_ringing = false;
        is_listening = false;
        is_calling = false;
        is_alert = false;
        safe_mode = false;
        person_inside = InsideHouse();
        GameObject.Find("Diagnostics").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        camera_pos = Camera.main.transform.position;
        person_inside_previous = person_inside;
        person_inside = InsideHouse();
        if (!person_inside_previous && person_inside && !door_open && alert_controller.alert_enable)
        { // intruder && enable smart alert
            alert_controller.AlertStart();
        }

        if (door_open)
        {
            lightRenderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            lightRenderer.material.SetColor("_Color", Color.red);
        }

        switch (status)
        {
            case Status.ALERT:
                panel_screen.SetButtons(0);
                text_printer.Loginfo("???????????????!");
                system_buttons.current_button = 0;
                break;
            case Status.SAFETY_MODE:
                panel_screen.SetButtons(1, "??????????????????");
                text_printer.Loginfo("?????????????????????!");
                system_buttons.current_button = 0;
                break;
            case Status.IDLE:
                panel_screen.SetButtons(0);
                system_buttons.current_button = 0;
                break;
            case Status.COMMON_MAIN:
                panel_screen.SetButtons(3, "????????????", "????????????", "????????????");
                system_buttons.current_button = 1;
                break;
            case Status.COMMON_RING_SETTING:
                text_printer.Loginfo("????????????:" + (ring_controller.current_ring == 0 ? ring_controller.ring_names[0] : (ring_controller.current_ring == 1 ? ring_controller.ring_names[1] : ring_controller.ring_names[2]))
                + "\n" + "????????????:" + ((int)ring_controller.current_volume).ToString());
                panel_screen.SetButtons(3, ring_controller.ring_names[0], ring_controller.ring_names[1], ring_controller.ring_names[2]);
                system_buttons.current_button = 1;
                break;
            case Status.COMMON_CALL_SETTING:
                text_printer.Loginfo("????????????:" + (call_controller.current_call == 0 ? call_controller.call_names[0] : call_controller.call_names[1])
                + "\n" + "????????????:" + ((int)call_controller.current_volume).ToString());
                panel_screen.SetButtons(2, call_controller.call_names[0], call_controller.call_names[1]);
                system_buttons.current_button = 1;
                break;
            case Status.MANAGE_MAIN:
                panel_screen.SetButtons(2, "????????????", "???????????????");
                system_buttons.current_button = 2;
                break;
            case Status.MANAGE_PASSWORD_CHECK:
                if (password_controller.status == PasswordStatus.FAIL)
                {
                    text_printer.Loginfo("????????????,????????????????????????");
                }
                else if (password_controller.status == PasswordStatus.SUCCESS)
                {
                    status = Status.MANAGE_MANAGER_CHOOSE;
                }
                else
                {
                    text_printer.Loginfo("???????????????????????????????????????");
                }
                panel_screen.SetButtons(0);
                system_buttons.current_button = 2;
                break;
            case Status.MANAGE_MANAGER_CHOOSE:
                text_printer.Loginfo("????????????????????????");
                panel_screen.SetButtons(3, manage_controller.names[0], manage_controller.names[1], manage_controller.names[2]);
                system_buttons.current_button = 2;
                break;
            case Status.SAFETY_MAIN:
                panel_screen.SetButtons(2, "????????????", "??????????????????");
                system_buttons.current_button = 3;
                break;
            case Status.SAFETY_MODE_ENTER_CHECK:
                if (password_controller.status == PasswordStatus.FAIL)
                {
                    text_printer.Loginfo("????????????,????????????????????????");
                }
                else if (password_controller.status == PasswordStatus.SUCCESS)
                {
                    status = Status.SAFETY_MODE;
                    safe_mode = true;
                }
                else
                {
                    text_printer.Loginfo("???????????????????????????????????????");
                }
                panel_screen.SetButtons(0);
                system_buttons.current_button = 3;
                break;
            case Status.SAFETY_MODE_EXIT_CHECK:
                if (password_controller.status == PasswordStatus.FAIL)
                {
                    text_printer.Loginfo("????????????,????????????????????????");
                }
                else if (password_controller.status == PasswordStatus.SUCCESS)
                {
                    status = Status.SAFETY_MAIN;
                    safe_mode = false;
                }
                else
                {
                    text_printer.Loginfo("???????????????????????????????????????");
                }
                panel_screen.SetButtons(0);
                system_buttons.current_button = 0;
                break;
            case Status.SAFETY_SETTING:
                text_printer.Loginfo("????????????????????????:" + (alert_controller.alert_enable ? "??????" : "??????"));
                panel_screen.SetButtons(2, (alert_controller.alert_enable ? "??????" : "??????") + "????????????", "????????????");
                system_buttons.current_button = 3;
                break;
            case Status.SAFETY_PASSWORD_CHECK:
                if (password_controller.status == PasswordStatus.FAIL)
                {
                    text_printer.Loginfo("????????????,????????????????????????");
                }
                else if (password_controller.status == PasswordStatus.SUCCESS)
                {
                    status = Status.SAFETY_PASSWORD_OVERRIDE;
                    password_controller.OverridePassword();
                }
                else
                {
                    text_printer.Loginfo("???????????????????????????????????????");
                }
                panel_screen.SetButtons(0);
                system_buttons.current_button = 3;
                break;
            case Status.SAFETY_PASSWORD_OVERRIDE:
                if (password_controller.status == PasswordStatus.SUCCESS)
                {
                    status = Status.SAFETY_SETTING;
                }
                else
                {
                    text_printer.Loginfo("??????????????????????????????????????????");
                }
                panel_screen.SetButtons(0);
                system_buttons.current_button = 3;
                break;
            case Status.OTHERS_MAIN:
                text_printer.Loginfo("??????????????????????????????:" + (light_controller.smart_light_trigger ? "??????" : "??????") + "\n"
                                    + "????????????????????????:" + (light_controller.outside_light_on ? "??????" : "??????"));
                panel_screen.SetButtons(2, (light_controller.smart_light_trigger ? "??????" : "??????") + "????????????????????????", (light_controller.outside_light_on ? "??????" : "??????") + "????????????");
                system_buttons.current_button = 4;
                break;
        }
    }

    public bool InsideHouse()
    {
        if ((camera_pos.z >= house_minz && camera_pos.z < house_midz && camera_pos.x >= house_minx && camera_pos.x < house_maxx)
        || (camera_pos.z >= house_midz && camera_pos.z < house_maxz && camera_pos.x >= house_minx && camera_pos.x < house_midx))
            return true;
        return false;
    }

    // system_buttons leftside, valid when system on
    public void CommonButtonPressed()
    {
        if (enable && !is_alert && !safe_mode)
        {
            text_printer.Clear();
            ring_controller.RingStop();
            status = Status.COMMON_MAIN;
        }
    }
    public void ManageButtonPressed()
    {
        if (enable && !is_alert && !safe_mode)
        {
            text_printer.Clear();
            ring_controller.RingStop();
            status = Status.MANAGE_MAIN;
        }
    }
    public void SafetyButtonPressed()
    {
        if (enable && !is_alert && !safe_mode)
        {
            text_printer.Clear();
            ring_controller.RingStop();
            status = Status.SAFETY_MAIN;
        }
    }
    public void OthersButtonPressed()
    {
        if (enable && !is_alert && !safe_mode)
        {
            text_printer.Clear();
            ring_controller.RingStop();
            status = Status.OTHERS_MAIN;
        }

    }

    // basic_buttons rightside, always valid
    public void DoorOpenButtonPressed()
    {
        if (!door_open)
        {
            TriggerDoorOpen();
        }
        else
        {
            TriggerDoorClose();
        }
    }
    public void DoorLockButtonPressed()
    {
        if (!door_open)
        {
            if (door_lock == false)
            {
                text_printer.Loginfo("????????????");
                door_lock = true;
            }
            else
            {
                text_printer.Loginfo("????????????");
                door_lock = false;
            }
        }
        else
        {
            text_printer.Loginfo("????????????????????????");
        }
    }
    public void ListenButtonPressed()
    {
        if (is_listening)
        {
            is_listening = false;
            text_printer.Loginfo("????????????");
        }
        else if (bell_ringing)
        {
            bell_ringing = false;
            is_listening = true;
            ring_controller.RingStop();
            text_printer.Loginfo("?????????");
        }
        else
        {
            text_printer.Loginfo("????????????????????????");
        }
    }
    public void CallButtonPressed()
    {
        if (is_calling)
        {
            is_calling = false;
            call_controller.CallStop();
            text_printer.Loginfo("????????????");
        }
        else
        {
            is_calling = true;
            text_printer.Loginfo("??????" + call_controller.call_names[call_controller.current_call]);
            call_controller.CallStart();
        }
    }
    public void SystemButtonPressed()
    {
        if (!enable)
        {
            enable = true;
            Boot();
        }
        else
        {
            enable = false;
            ShutDown();
        }
    }
    public void AlertButtonPressed()
    {
        if (enable && !is_alert)
        {
            alert_controller.alert_count += 100;
        }
    }

    // panel_buttons on the panel, valid when system on
    public void Button1Pressed()
    {
        if (enable && !is_alert)
        {
            switch (status)
            {
                case Status.COMMON_MAIN:
                    text_printer.Loginfo("??????????????????v1.4\n??????????????????:??????\n????????????:??????");
                    break;
                case Status.COMMON_RING_SETTING:
                    ring_controller.current_ring = 0;
                    ring_controller.RingStart();
                    break;
                case Status.COMMON_CALL_SETTING:
                    call_controller.current_call = 0;
                    break;
                case Status.MANAGE_MAIN:
                    text_printer.Loginfo("????????????:\n" + manage_controller.names[0] + (manage_controller.manager_id == 0 ? "(?????????)\n" : "\n")
                    + manage_controller.names[1] + (manage_controller.manager_id == 1 ? "(?????????)\n" : "\n")
                    + manage_controller.names[2] + (manage_controller.manager_id == 2 ? "(?????????)\n" : "\n"));
                    break;
                case Status.MANAGE_MANAGER_CHOOSE:
                    manage_controller.manager_id = 0;
                    text_printer.Loginfo("?????????????????????");
                    status = Status.MANAGE_MAIN;
                    break;
                case Status.SAFETY_MAIN:
                    status = Status.SAFETY_SETTING;
                    break;
                case Status.SAFETY_SETTING:
                    alert_controller.alert_enable = !alert_controller.alert_enable;
                    break;
                case Status.SAFETY_MODE:
                    status = Status.SAFETY_MODE_EXIT_CHECK;
                    password_controller.CheckPassword();
                    break;
                case Status.OTHERS_MAIN:
                    light_controller.smart_light_trigger = !light_controller.smart_light_trigger;
                    break;
                default:
                    break;
            }
        }
    }
    public void Button2Pressed()
    {
        if (enable && !is_alert)
        {
            switch (status)
            {
                case Status.COMMON_MAIN:
                    status = Status.COMMON_RING_SETTING;
                    break;
                case Status.COMMON_RING_SETTING:
                    ring_controller.current_ring = 1;
                    ring_controller.RingStart();
                    break;
                case Status.COMMON_CALL_SETTING:
                    call_controller.current_call = 1;
                    break;
                case Status.MANAGE_MAIN:
                    status = Status.MANAGE_PASSWORD_CHECK;
                    password_controller.CheckPassword();
                    break;
                case Status.MANAGE_MANAGER_CHOOSE:
                    manage_controller.manager_id = 1;
                    text_printer.Loginfo("?????????????????????");
                    status = Status.MANAGE_MAIN;
                    break;
                case Status.SAFETY_MAIN:
                    status = Status.SAFETY_MODE_ENTER_CHECK;
                    password_controller.CheckPassword();
                    break;
                case Status.SAFETY_SETTING:
                    status = Status.SAFETY_PASSWORD_CHECK;
                    password_controller.CheckPassword();
                    break;
                case Status.OTHERS_MAIN:
                    light_controller.outside_light_on = !light_controller.outside_light_on;
                    break;
                default:
                    break;
            }
        }
    }
    public void Button3Pressed()
    {
        if (enable && !is_alert)
        {
            switch (status)
            {
                case Status.COMMON_MAIN:
                    status = Status.COMMON_CALL_SETTING;
                    break;
                case Status.COMMON_RING_SETTING:
                    ring_controller.current_ring = 2;
                    ring_controller.RingStart();
                    break;
                case Status.MANAGE_MANAGER_CHOOSE:
                    manage_controller.manager_id = 2;
                    text_printer.Loginfo("?????????????????????");
                    status = Status.MANAGE_MAIN;
                    break;
                default:
                    break;
            }
        }
    }
    public void BackButtonPressed()
    {
        if (enable && !is_alert)
        {
            ring_controller.RingStop();
            text_printer.Clear();
            switch (status)
            {
                case Status.COMMON_CALL_SETTING:
                case Status.COMMON_RING_SETTING:
                    status = Status.COMMON_MAIN;
                    break;
                case Status.MANAGE_PASSWORD_CHECK:
                    status = Status.MANAGE_MAIN;
                    break;
                case Status.SAFETY_SETTING:
                case Status.SAFETY_MODE_ENTER_CHECK:
                    status = Status.SAFETY_MAIN;
                    break;
                case Status.SAFETY_PASSWORD_CHECK:
                case Status.SAFETY_PASSWORD_OVERRIDE:
                    status = Status.SAFETY_SETTING;
                    break;
                case Status.SAFETY_MODE_EXIT_CHECK:
                    status = Status.SAFETY_MODE;
                    break;
                default:
                    break;
            }
        }
    }

    // system_buttons on the outsidepanel
    public void RingButtonPressed()
    {
        if (bell_ringing && !is_listening)
        {
            bell_ringing = false;
            ring_controller.RingStop();
        }
        else if (!bell_ringing && !is_listening)
        {
            bell_ringing = true;
            ring_controller.RingStart();
        }
    }
    public void OutSideDoorButtonPressed()
    {
        if (!door_open)
        {
            TriggerDoorOpen();
        }
        else
        {
            TriggerDoorClose();
        }
    }

    public void LightSwitchPressed()
    {
        light_controller.inside_light_on = !light_controller.inside_light_on;
    }

    // utils

    public void Boot()
    { // open the system
        text_printer.Loginfo("???????????????");
        status = safe_mode ? Status.SAFETY_MODE : Status.IDLE;
    }

    public void ShutDown()
    { // close the system
        text_printer.Clear();
        alert_controller.AlertStop(); // only this can stop the alert
        status = safe_mode ? Status.SAFETY_MODE : Status.IDLE;
    }

    public void TriggerDoorOpen()
    {
        if (!safe_mode)
        {// if in safe mode, door would not open
            if (door_open == false && door_lock == false)
            {
                door_open = true;
                door_anim.Open();
                text_printer.Loginfo("????????????");
            }
            else if (door_open == false && door_lock == true)
            {
                text_printer.Loginfo("????????????");
            }
        }
    }

    public void TriggerDoorClose()
    {
        if (door_open == true)
        {
            door_open = false;
            door_anim.Close();
            text_printer.Loginfo("????????????");
        }
    }
}
