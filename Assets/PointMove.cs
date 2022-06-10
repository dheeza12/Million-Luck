using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMove : MonoBehaviour
{
    public void MoveToward(Vector3 target)
    {
        LeanTween.move(gameObject, target, 1.67f).setEaseInOutCubic().setOnComplete(Inactive).delay = 0.1f;
    }

    private void Inactive()
    {
        gameObject.SetActive(false);
    }
}
