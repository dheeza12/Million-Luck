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
    private WaitForUIButtons waitWaypointChoice;
    private WaitForUIButtons waitBattleChoice;
    private Button[] buttonsWaypoint;
    private Button[] buttonsBattle;

    private void Start() {  
        playerAttribute = GetComponent<PlayerAttribute>();
        for (int i = 0; i < BoardIterate.boardCount; i++)
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
        waitWaypointChoice = new WaitForUIButtons(ButtonsManager.Instance.buttonsWaypoint);
        waitBattleChoice = new WaitForUIButtons(ButtonsManager.Instance.buttonsBattle);
        buttonsWaypoint = ButtonsManager.Instance.buttonsWaypoint;
        buttonsBattle = ButtonsManager.Instance.buttonsBattle;

    }

    public void StartMove(){
        StartCoroutine(Move());

        // Coroutine act in parallel so any subsequence script here will be running atm
    }

    public IEnumerator Move() {
        
        int endIndex = startWaypoint + GameController.diceSideThrown;
        int step = GameController.diceSideThrown;
        bool newEndIndex;
        bool nextTurn = false;

        // others' waypointIndex/startWaypoint to compare
        Transform[] players_list = GameController.players_ingame;
        int[] player_list_waypoint_index = new int[4];
        for (int i = 0; i < player_list_waypoint_index.Length; i++)
        {
            if (players_list[i] != null)
            {
                player_list_waypoint_index[i] = players_list[i].GetComponent<Path>().startWaypoint;
            }
        }
        
        while (!nextTurn) {
            int destination = (waypointIndex) % BoardIterate.boardCount;
            // Check when player position is at destination block

            // Stop Position when battling
            if (!GameController.battleInProgress)
            {
                if (endIndex == waypointIndex - 1)
                {
                    startWaypoint = waypointIndex - 1;
                    // Waypoints store special block pointers
                    BlockType currentBlockType = BoardIterate.childsGameObject[startWaypoint % BoardIterate.boardCount].GetComponent<Waypoints>().blockType;
                    // Check if action done
                    nextTurn = GameController.Instance.BlockActionDone(currentBlockType);
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
                    // Compare waypoint between players
                    if (!GameController.battleInProgress)
                    {
                        for (int i = 0; i < player_list_waypoint_index.Length; i++)
                        {
                            // exclude self and null
                            if (i != GameController.whoseTurn - 1 & players_list[i] != null)
                            {
                                if (player_list_waypoint_index[i] == waypointIndex & step < GameController.diceSideThrown)
                                {
                                    GameController.attacker = GameController.whoseTurn;
                                    GameController.gettingAttacked = i + 1;

                                    // Activating prompt to ask if battle
                                    ButtonsManager.Instance.battleDesuka.SetActive(true);
                                    // Wait for any Button pressed
                                    yield return waitBattleChoice.Reset();
                                    ButtonsManager.Instance.battleDesuka.SetActive(false);
                                    if (waitBattleChoice.PressedButton == buttonsBattle[0])
                                    {
                                        yield return StartCoroutine(GameController.Instance.PlayerBattle(opponentNumber: i));
                                        endIndex = player_list_waypoint_index[i];
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
                            endIndex = (waypointIndex - 1) + step;
                        }
                        else if (available_waypoints.CountWaypoints() > 1)
                        {
                            // Intersection
                            for (int i = 0; i < available_waypoints.CountWaypoints(); i++)
                            {
                                buttonsWaypoint[i].gameObject.SetActive(true);
                                // Button shown may be wrong when passing multiple intersection
                                int posShowButtons = (available_waypoints.waypoints[i].indexNumber - 1 + step) % BoardIterate.boardCount;
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
                            endIndex = (waypointIndex - 1) + step;
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
            }

            yield return new WaitForEndOfFrame();
        }
        // Next turn
        // wayppointIndex point to the next block, we want it to be at start when next turn
        waypointIndex--;
        GameController.Instance.NextTurn();
       
    }    
}
