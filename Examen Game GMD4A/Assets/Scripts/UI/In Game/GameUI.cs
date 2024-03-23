using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public Input inputs;

    public Transform leaderboardPanel;
    public GameObject leaderboardPanelPrefab;

    public TMP_Text stuckText;
    public Slider healthSlider;

    public TMP_Text countDownTime;
    public FinishUI finishUI;

    List<LeaderboardItem> leaderboardList;
    private void Awake()
    {
        instance = this;
    }

    public void StuckNotification(bool on)
    {
        stuckText.transform.parent.gameObject.SetActive(on);

        if(!on)
            return;

        //notification
        //if(inputs == "Keyboard")

        stuckText.text = "Hold" + " F " + "To Respawn at Last Checkpoint";
    }
    public IEnumerator ShowHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
        healthSlider.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(5);
        healthSlider.transform.parent.gameObject.SetActive(false);
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
