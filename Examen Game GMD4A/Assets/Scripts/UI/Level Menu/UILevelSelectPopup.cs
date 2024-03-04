using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelectPopup : MonoBehaviour
{

    public Level currentLevel;
    public TMP_Text missionText, descriptionText;

    public void ShowPopup(Button button)
    {
        currentLevel = LevelManager.INSTANCE.GetLevel(button.transform.GetSiblingIndex());
        missionText.text = currentLevel.levelName;
        descriptionText.text = currentLevel.levelDescription;
        gameObject.SetActive(true);
    }

    public void PlayCurrent()
    {
        LevelManager.INSTANCE.LoadLevel(currentLevel);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }

}
