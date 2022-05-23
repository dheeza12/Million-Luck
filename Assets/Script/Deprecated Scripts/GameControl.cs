using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static int numPlayer = 4;
    private static GameObject[] players = new GameObject[4];
    
    public static int diceSideThrown = 0;
    public static int[] playersStartWaypoint = new int[] {0, 0, 0, 0};
    
    public static bool gameOver = false;


    private void Start() {
        int j = 0;
        for (int i = 0; i < numPlayer; i++)
        {
            players[i] = GameObject.Find("Player" + (1+i).ToString());

            if (players[i] != null)
            {              
                players[i].GetComponent<FollowPath>().moveAllowed = false;
                // Debug.Log("GameControl: " + players[i].GetComponent<FollowPath>().waypointIndex);
                playersStartWaypoint[i] = players[i].GetComponent<FollowPath>().waypointIndex;
                j++;
            }

        }
        numPlayer = j;
        
    }

    private void Update() {
        Debug.Log(playersStartWaypoint[0]);
    }

    public static void MovePlayer(int playerToMove){
        
        switch (playerToMove)
        {
            case 1:
                players[0].GetComponent<FollowPath>().moveAllowed = true;
                break;
            case 2:
                players[1].GetComponent<FollowPath>().moveAllowed = true;
                break;
            
            case 3: 
                players[2].GetComponent<FollowPath>().moveAllowed = true;    
                break;

            case 4:
                players[3].GetComponent<FollowPath>().moveAllowed = true;
                break;

        }
    }
}
