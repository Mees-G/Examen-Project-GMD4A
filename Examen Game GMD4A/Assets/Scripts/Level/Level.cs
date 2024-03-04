using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "The Racing 20s/Level")]
public class Level : ScriptableObject
{

    public string sceneName;
    public string levelName;
    public string levelDescription;
    public int highscore = 0;
    public float fastestTime = 0.0F;

    public bool completed;
    public bool _completed
    {
        get
        {
            return completed;
        }
        set
        {
            onChangeCompleted.Invoke();
            completed = value;
        }
    }
    [HideInInspector]
    public Action onChangeCompleted;

}
