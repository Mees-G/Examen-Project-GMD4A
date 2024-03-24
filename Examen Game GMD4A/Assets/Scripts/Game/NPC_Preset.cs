using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "The Racing 20s/SpecialNPC")]
public class NPC_Preset : ScriptableObject
{
    public string npcName;
    public Image icon;

    public GameObject car;

    [Range(0, 1)] public float torqueMultiplier;
    [Range(0, 1)] public float topSpeedMultiplier;
    [Range(0, 1)] public float brakesMultiplier;
}
