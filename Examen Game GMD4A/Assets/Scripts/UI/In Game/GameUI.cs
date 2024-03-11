using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public Transform leaderboardPanel;
    public GameObject leaderboardPanelPrefab;

    //public FinishUI finishUI;

    List<LeaderboardItem> leaderboardList;
    private void Awake()
    {
        instance = this;
    }

    public void SetupLeaderBoard()
    {
        leaderboardList = new List<LeaderboardItem>();

        foreach (Controller_Base participant in RacerManager.instance.participants)
        {
            LeaderboardItem script =  Instantiate(leaderboardPanelPrefab, leaderboardPanel).GetComponent<LeaderboardItem>();
            
            script.controller = participant;

            if (script.controller.NPC)
            {
                //generate random name idk
                script.participantName.text = "NPC" + Random.Range(100, 999);
            }
            else
            {
                script.participantName.text = "Bob (Player)";
            }
            leaderboardList.Add(script);
        }
    }
    public void UpdateLeaderBoard(Controller_Base participant, int position)
    {
        for (int i = 0; i < leaderboardList.Count; i++)
        {
            if (leaderboardList[i].controller == participant)
            {
                leaderboardList[i].transform.SetSiblingIndex(position);
                break;
            }
        }
    }
}
