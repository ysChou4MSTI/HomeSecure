using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SystemButtons : MonoBehaviour
{
    [HideInInspector]
    public GameObject common;
    [HideInInspector]
    public GameObject manage;
    [HideInInspector]
    public GameObject safety;
    [HideInInspector]
    public GameObject others;

    [HideInInspector]
    public int current_button = 0;

    private Material transparent_blue;
    private Material original;

    private Material common_mtr;
    private Material manage_mtr;
    private Material safety_mtr;
    private Material others_mtr;

    // Start is called before the first frame update
    void Start()
    {
        common = GameObject.Find("CommonButton");
        manage = GameObject.Find("ManageButton");
        safety = GameObject.Find("SafetyButton");
        others = GameObject.Find("OthersButton");
        // transparent_blue = AssetDatabase.LoadAssetAtPath("Packages/com.microsoft.mixedreality.toolkit.standardassets/Materials/MRTK_Standard_TransparentBlue.mat", typeof(Material)) as Material;
        transparent_blue = common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material;
        original = manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material;

        // common_mtr=transparent_blue;
        // manage_mtr=transparent_blue;
        // safety_mtr=transparent_blue;
        // others_mtr=transparent_blue;

        // common_mtr.mainTexture = Resources.Load("common") as Texture;
        // manage_mtr.mainTexture = Resources.Load("management") as Texture;
        // safety_mtr.mainTexture = Resources.Load("security") as Texture;
        // others_mtr.mainTexture = Resources.Load("others") as Texture;
    }

    void Update()
    {
        switch (current_button)
        {
            case 0:
                common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                // common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = common_mtr;
                // manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = manage_mtr;
                // safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = safety_mtr;
                // others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = others_mtr;
                break;
            case 1:
                common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                // common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                // manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = manage_mtr;
                // safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = safety_mtr;
                // others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = others_mtr;
                break;
            case 2:
                common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                // common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = common_mtr;
                // manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                // safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = safety_mtr;
                // others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = others_mtr;
                break;
            case 3:
                common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                // common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = common_mtr;
                // manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = manage_mtr;
                // safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                // others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = others_mtr;
                break;
            case 4:
                common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = original;
                others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                // common.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = common_mtr;
                // manage.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = manage_mtr;
                // safety.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = safety_mtr;
                // others.GetComponentsInChildren<Transform>()[2].GetComponent<MeshRenderer>().material = transparent_blue;
                break;
            default:
                break;
        }
    }



}
