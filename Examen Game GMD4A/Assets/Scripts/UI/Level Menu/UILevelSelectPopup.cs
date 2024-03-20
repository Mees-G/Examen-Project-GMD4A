using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelSelectPopup : MonoBehaviour
{

    public UILevelPathHandler uiLevelPathHandler;
    public Level currentLevel;
    public Button button;
    public TMP_Text missionText, descriptionText;

    public void ShowPopup(UILevelSelectButton uiLevelSelectButton)
    {
        currentLevel = uiLevelSelectButton.level;
        missionText.text = currentLevel.levelName;
        descriptionText.text = currentLevel.levelDescription;
        gameObject.SetActive(true);
        this.button = uiLevelSelectButton.GetComponent<Button>();
    }

    public void PlayCurrent()
    {
        gameObject.SetActive(false);
        uiLevelPathHandler.StartCoroutine(LevelManager.INSTANCE.LoadLevel(currentLevel));
        GameManager.INSTANCE.latestLevelPosition = button.transform.localPosition;
        uiLevelPathHandler.shouldDoAnimation = true;
        Debug.Log("AA");
    }


    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }

}
