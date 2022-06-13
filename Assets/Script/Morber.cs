using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morber : MonoBehaviour
{
    public static Morber Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void StartMorbing(GameObject objectToMorb)
    {
        objectToMorb.transform.localScale = new Vector2(0.9f, 0.9f);
        objectToMorb.transform.LeanScale(Vector2.one, 0.33f).setEaseInOutCubic().setLoopPingPong();

    }

    public void StopMorbing(GameObject objectToMorb)
    {
        objectToMorb.transform.localScale = Vector2.one;
        LeanTween.cancel(objectToMorb);
    }

}
