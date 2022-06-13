using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardIterate : MonoBehaviour
{
    public static Transform[] childsTransform;
    public static GameObject[] childsGameObject;

    private void Awake()
    {
        Transform[] loadOut = transform.GetComponentsInChildren<Transform>();
        // GetComponentInChildren<Transform>() always include the parent aswell, the actual Length of Children would be -1
        int size = loadOut.Length - 1;

        childsTransform = new Transform[size];
        childsGameObject = new GameObject[size];
        int i = 0;

        foreach (Transform child in loadOut)
        {
            // exclude the parent
            if (child != transform)
            {
                childsTransform[i] = child;
                childsGameObject[i] = child.gameObject;
                childsGameObject[i].GetComponent<Waypoints>().indexNumber = i;
                i++;
            }
        }
    }

    public static int boardCount { get { return childsTransform.Length; } }
    

}
