using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishUI : MonoBehaviour
{

    [SerializeField]
    private TMP_Text placeText;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text timeText;


    public void ShowFinishUI(string place, int score, string time)
    {
        SetPlace(place);
        SetScore(score);
        SetTime(time);
        gameObject.SetActive(true);
    }

    public void SetPlace(string place)
    {
        placeText.text = "#" + place;
    }

    public void SetScore(int score)
    {
        scoreText.SetValue(this, score, 1.0F);
    }

    public void SetTime(string time)
    {
        timeText.text = time;
    }

    public void ButtonClickContinue()
    {
        SceneManager.LoadScene("Store");
    }

}
