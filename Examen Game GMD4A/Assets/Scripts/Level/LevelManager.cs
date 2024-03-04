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


    public List<Level> levels = new List<Level>();

    public event System.Action<int> onChangeLevelIndex;

    private int _currentLevelIndex;
    public int currentLevelIndex
    {
        get { return _currentLevelIndex; }
        set
        {
            if (_currentLevelIndex != value)
            {
                _currentLevelIndex = value;

                Debug.Log(onChangeLevelIndex + " - " + _currentLevelIndex);
                onChangeLevelIndex.Invoke(_currentLevelIndex);
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
                level.onChangeCompleted += UpdateCurrentLevelIndex;
            }
        }
    }

    public void UpdateCurrentLevelIndex()
    {
        for (int i = 0; i < levels.Count - 1; i++)
        {
            if (!levels[i].completed)
            {
                currentLevelIndex = i;
                return;
            }
        }
        int newIndex = levels.Count - 1;
        currentLevelIndex = newIndex;
    }

    public Level GetLevel(int index)
    {
        return levels[index];
    }

    public void LoadLevel(Level level)
    {
        SceneManager.LoadScene(level.sceneName);
    }

    public void LoadLevel(int index)
    {
        LoadLevel(GetLevel(index));
    }


}
