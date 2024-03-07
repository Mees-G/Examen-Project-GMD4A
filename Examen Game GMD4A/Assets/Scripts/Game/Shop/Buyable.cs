using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "The Racing 20s/Buyable")]
public class Buyable : ScriptableObject
{
    public string _name = "Basic Car";
    public string description = "A Basic Car, nothing special yet...";
    
    public GameObject car;

    public Sprite icon;


    public int price = 100;

    public bool unlocked;

    //public Dictionary<string, UpgradeData> upgrades;

        
   
}
