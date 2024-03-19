using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{

    private static LevelManager _INSTANCE;
    public static LevelManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = new LevelManager();
            }
            return _INSTANCE;
        }
        set { _INSTANCE = value; }
    }


    [HideInInspector]
    public List<Level> levels;
    public System.Action<int> onLevelCompleted = delegate { };

    private int _completedLevelCount;
    public int completedLevelCount
    {
        get {
            //Debug.Log(_currentLevelIndex +" - AAA");
            return _completedLevelCount; }
        set
        {
            if (_completedLevelCount != value)
            {
                _completedLevelCount = value;

                //Debug.Log(onChangeLevelIndex + " - " + _currentLevelIndex);
                onLevelCompleted.Invoke(_completedLevelCount);  
            }
        }
    }

    public LevelManager()
    {
        levels = new List<Level>();
        Object[] loadedObjects = Resources.LoadAll("Levels", typeof(Level));
        foreach (Object loadedObject in loadedObjects)
        {
            if (loadedObject != null && loadedObject is Level)
            {
                Level level = loadedObject as Level;
                levels.Add(loadedObject as Level);
                if (level.completed)
                {
                    completedLevelCount++;
                }
                else
                {
                    level.onChangeCompleted += UpdateCurrentLevelIndex;
                }
            }
        }
    }

    public void UpdateCurrentLevelIndex()
    {
        for (int i = 0; i < levels.Count - 1; i++)
        {
            if (levels[i].completed)
            {
                completedLevelCount++;
            }
        }
    }

    public Level GetLevel(int index)
    {
        return levels[index];
    }

    public void LoadLevel(Level level)
    {
        GameManager.INSTANCE.currentLevel = level;
        SceneManager.LoadScene(level.sceneName);
    }

    public void LoadLevel(int index)
    {
        LoadLevel(GetLevel(index));
    }


}
