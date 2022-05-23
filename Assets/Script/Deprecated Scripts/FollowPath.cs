using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] public int waypointIndex;  
    public bool moveAllowed = false;

    [SerializeField] private GameObject board;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int contestantNumber = 1;

    private void Start() {  
        string starterBlockName = "Starter" + contestantNumber.ToString();

        for (int i = 0; i < BoardIterate.CountIndex(); i++)
        {
            if (starterBlockName == BoardIterate.childsTransform[i].name)
            {
                transform.position = BoardIterate.childsTransform[i].position;
                waypointIndex = i;
                // Debug.Log(waypointIndex);
                // Debug.Log(waypointIndex);
            }
        }

        transform.position = board.transform.Find(starterBlockName).transform.position;
        
    }

    private void Update() {
        if (moveAllowed)
        {
            Move();
        }

    }


    private void Move() {
        
        int destination = waypointIndex % BoardIterate.CountIndex();
        int playerNum = contestantNumber - 1;
        int step = (GameControl.playersStartWaypoint[playerNum] + GameControl.diceSideThrown);

        if (transform.position == BoardIterate.childsTransform[destination].position)
        {
            waypointIndex += 1;

        }
        else if (!(waypointIndex > step))
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                                                BoardIterate.childsTransform[destination].position,
                                                moveSpeed * Time.deltaTime);
        }
        else
        {
            moveAllowed = false;
            GameControl.playersStartWaypoint[playerNum] = waypointIndex - 1;
        }
        
        
        
        
        
    }
    
}
