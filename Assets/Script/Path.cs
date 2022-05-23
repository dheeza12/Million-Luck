using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Enum for identifying block
using BlockTypeEnum;

public class Path : MonoBehaviour
{
    [SerializeField] public int waypointIndex;
    private int startWaypoint;

    [SerializeField] private float moveSpeed = 5f;

    [HideInInspector] public PlayerAttribute playerAttribute;
    [SerializeField] private ButtonsManager buttonsManager;
    private WaitForUIButtons waitWaypointChoice;
    private WaitForUIButtons waitBattleChoice;
    private Button[] buttonsWaypoint;
    private Button[] buttonsBattle;

    private void Start() {  
        playerAttribute = GetComponent<PlayerAttribute>();
        for (int i = 0; i < BoardIterate.CountIndex(); i++)
        {
            // Find starter block using loop to find name that match in Board
            // Starter1, Starter2, ... *might change to into an attribute at Waypoints*
            if (gameObject.name == BoardIterate.childsTransform[i].name)
            {
                transform.position = BoardIterate.childsTransform[i].position;
                waypointIndex = i;
                startWaypoint = i;
            }
        }
        buttonsWaypoint = buttonsManager.buttonsWaypoint;
        buttonsBattle = buttonsManager.buttonsBattle;
        waitWaypointChoice = new WaitForUIButtons(buttonsWaypoint);
        waitBattleChoice = new WaitForUIButtons(buttonsBattle);
        
    }

    public void StartMove(){
        StartCoroutine(Move());

        // Coroutine act in parallel so any subsequence script here will be running atm
    }

    public IEnumerator Move() {
        
        int endIndex = startWaypoint + GameController.diceSideThrown + 1;
        int step = GameController.diceSideThrown;
        bool newEndIndex = false;
        bool nextTurn = false;

        // others' waypointIndex/startWaypoint to compare
        Transform[] players_list = GameController.players_ingame;
        int[] player_list_waypoint_index = new int[4];
        for (int i = 0; i < players_list.Length; i++)
        {
            if (players_list[i] != null)
            {
                player_list_waypoint_index[i] = players_list[i].GetComponent<Path>().startWaypoint;
            }
        }
        
        while (!nextTurn) {
            int destination = waypointIndex % BoardIterate.CountIndex();
            // Check when player position is at destination block

            if (endIndex == waypointIndex)
            {
                waypointIndex--;
                startWaypoint = waypointIndex;
                
                // Waypoints store special block pointers
                BlockType currentBlockType = BoardIterate.childsGameObject[startWaypoint % BoardIterate.CountIndex()].GetComponent<Waypoints>().blockType;
                // Check if action done
                nextTurn = GameController.ActionDone(currentBlockType);
            }
            else if (transform.position == BoardIterate.childsTransform[destination].position)
            {
                Waypoints available_waypoints = BoardIterate.childsGameObject[destination].GetComponent<Waypoints>();
                
                if (available_waypoints.CountWaypoints() > 0 & step > 0)
                {
                    newEndIndex = true;
                }
                else
                {
                    newEndIndex = false;
                }
                
                // Compare waypoint
                if (!GameController.battleInProgress)
                {
                    for (int i = 0; i < players_list.Length; i++)
                    {
                        if (i != GameController.whoseTurn - 1 & players_list[i] != null)
                        {
                            if (player_list_waypoint_index[i] == waypointIndex & step < GameController.diceSideThrown)
                            {
                                GameController.attacker = GameController.whoseTurn;
                                GameController.gettingAttacked = i + 1;

                                buttonsManager.battleDesuka.SetActive(true);
                                // Wait for any Button pressed
                                yield return waitBattleChoice.Reset();
                                buttonsManager.battleDesuka.SetActive(false);
                                if (waitBattleChoice.PressedButton == buttonsBattle[0])
                                {
                                    GameController.Battle(isPlayer: true, opponentNumber: i);

                                    endIndex = player_list_waypoint_index[i] + 1;
                                    newEndIndex = false;
                                }
                                
                                
                            }
                        }
                    }
                }
                

                if (newEndIndex)
                {
                    if (available_waypoints.CountWaypoints() == 1)
                    {
                        waypointIndex = available_waypoints.waypoints[0].indexNumber;
                        endIndex = waypointIndex + step;
                    }
                    else if (available_waypoints.CountWaypoints() > 1)
                    {
                        // Intersection
                        for (int i = 0; i < available_waypoints.CountWaypoints(); i++)
                        {
                            buttonsWaypoint[i].gameObject.SetActive(true);
                            // Button shown may be wrong when passing multiple intersection
                            int posShowButtons = (available_waypoints.waypoints[i].indexNumber + step - 1) % BoardIterate.CountIndex();
                            buttonsWaypoint[i].transform.position = BoardIterate.childsTransform[posShowButtons].transform.position;
                        }
                        yield return waitWaypointChoice.Reset();
                        if (waitWaypointChoice.PressedButton == buttonsWaypoint[0])
                        {
                            waypointIndex = available_waypoints.waypoints[0].indexNumber;
                        }
                        else if (waitWaypointChoice.PressedButton == buttonsWaypoint[1])
                        {
                            waypointIndex = available_waypoints.waypoints[1].indexNumber;
                        }
                        endIndex = waypointIndex + step;
                        foreach (Button button in buttonsWaypoint)
                        {
                            button.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    waypointIndex++;    
                }
                step--;
                yield return new WaitForSeconds(0.22f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, BoardIterate.childsTransform[destination].position, moveSpeed * Time.deltaTime);
            }

            yield return new WaitForEndOfFrame();
        }
        // Next turn
        GameController.NextTurn();
       
    }    
}
