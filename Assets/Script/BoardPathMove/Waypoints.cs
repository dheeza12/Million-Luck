using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockTypeEnum;

public class Waypoints : MonoBehaviour
{
    public Waypoints[] waypoints;
    public int indexNumber;
    public BlockType blockType;

    private void Start() {
        if (GetComponent<SpriteRenderer>())
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();

            // Changing Sprite, *Temp
            if (blockType == BlockType.Lucky)
            {
                // Yellow-ish
                rend.color = new Color32(255, 234, 167, 255);
            }
            else if (blockType == BlockType.Unlucky)
            {
                // Purple-ish
                rend.color = new Color32(162, 155, 254, 255);
            }
            else if (blockType == BlockType.Quiz)
            {
                // Red-ish
                rend.color = new Color32(255, 118, 117, 255);
            }
            else if (blockType == BlockType.DoubleMove)
            {
                // Blue-ish
                rend.color = new Color32(116, 185, 255, 255);
            }
        }
        
    }

    public int CountWaypoints(){
        return waypoints.Length;
    }
}
