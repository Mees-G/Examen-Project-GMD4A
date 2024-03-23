using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "The Racing 20s/Level")]
public class Level : ScriptableObject
{
    public string sceneName;
    public string levelName;
    [TextArea] public string levelDescription;
    public int highscore = 0;
    public float fastestTime = 0.0F;

    public Level parentLevel;
    public LevelType levelType;
    public TimeOfDay timeOfDay;
    public int trackIndex;
    public List<GameObject> NPC_Cars;
    public List<GameObject> SPECIAL_NPC_Cars;
    //npc difficulty value?
    //generic NPC list?
    //special npc list?
    //

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
