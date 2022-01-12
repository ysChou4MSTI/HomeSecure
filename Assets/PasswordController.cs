using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public enum PasswordStatus
{
    IDLE,
    CHECK,
    OVERRIDE,
    FAIL,
    SUCCESS
};
public class PasswordController : MonoBehaviour
{
    public MonitorSystem monitor_system;
    [HideInInspector]
    public List<int> password;
    [HideInInspector]
    public PasswordStatus status;
    [HideInInspector]
    public int number;
    public XRNode inputSource;
    public InputHelpers.Button inputButton1;
    public InputHelpers.Button inputButton2;

    private bool press1 = false;
    private bool press2 = false;


    void Start()
    {
        password = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            password.Add(0);
        }
        status = PasswordStatus.IDLE;
    }

    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton1, out bool isPressed1); // X button
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton2, out bool isPressed2); // Y button
        switch (monitor_system.status)
        {
            case Status.SAFETY_PASSWORD_CHECK:
            case Status.SAFETY_PASSWORD_OVERRIDE:
            case Status.SAFETY_MODE_ENTER_CHECK:
            case Status.SAFETY_MODE_EXIT_CHECK:
            case Status.MANAGE_PASSWORD_CHECK:
                break;
            default:
                status = PasswordStatus.IDLE;
                break;
        }
        if (status == PasswordStatus.CHECK)
        {
            if (!isPressed1 && press1)
            { // X, 0
                if (password[number] == 0)
                {//correct
                    number++;
                    if (number == 3)
                    {//finish
                        status = PasswordStatus.SUCCESS;
                    }
                }
                else
                {//wrong
                    status = PasswordStatus.FAIL;
                }
            }
            else if (!isPressed2 && press2)
            { // Y, 1
                if (password[number] == 1)
                {//correct
                    number++;
                    if (number == 3)
                    {//finish
                        status = PasswordStatus.SUCCESS;
                    }
                }
                else
                {//wrong
                    status = PasswordStatus.FAIL;
                }
            }
        }
        else if (status == PasswordStatus.OVERRIDE)
        {
            if (!isPressed1 && press1)
            { // X, 0
                password[number] = 0;
                number++;
                if (number == 3)
                {//finish
                    status = PasswordStatus.SUCCESS;
                }
            }
            else if (!isPressed2 && press2)
            { // Y, 1
                password[number] = 1;
                number++;
                if (number == 3)
                {//finish
                    status = PasswordStatus.SUCCESS;
                }
            }
        }
        press1 = isPressed1;
        press2 = isPressed2;
    }

    public void CheckPassword()
    {
        status = PasswordStatus.CHECK;
        number = 0;
    }

    public void OverridePassword()
    {
        status = PasswordStatus.OVERRIDE;
        number = 0;
    }
}
