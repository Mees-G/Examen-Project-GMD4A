using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "The Racing 20s/Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("General Info")]
    public string upgradeName;

    [Header("Price")]
    public int upgradePrice = 50;
    public float priceLevelIncrement = 5;

    [Header("Upgrade Values")]

    [SerializeField]
    private int _currentValue;

    [HideInInspector]
    public int currentValue
    {
        get { return _currentValue; }
        set
        {
            _currentValue = value;
            if (_currentValue > upgradeValueMax)
            {
                _currentValue = (int)upgradeValueMax;
            }
            else if (_currentValue < upgradeValueMin)
            {
                _currentValue = (int)upgradeValueMin;
            }
        }
    }

    [HideInInspector]
    public float upgradeValueMin;

    [HideInInspector]
    public float upgradeValueMax;

    public int CalculateCurrentPrice()
    {
        double finalPrice = upgradePrice + (priceLevelIncrement * currentValue);
        return (int) finalPrice;
    }

    public T GetValue<T>()
    {
        return (T)(object)currentValue;
    }
}

[CustomEditor(typeof(Upgrade))]
public class UpgradeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Upgrade upgrade = (Upgrade)target;

        // Draw default inspector for other properties
        DrawDefaultInspector();

        // Draw min-max slider for upgradeValueMin and upgradeValueMax
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        upgrade.upgradeValueMin = (int)upgrade.upgradeValueMin;
        upgrade.upgradeValueMax = (int)upgrade.upgradeValueMax;
        EditorGUILayout.MinMaxSlider(new GUIContent("Range (" + ((int)upgrade.upgradeValueMin).ToString() + " - " + ((int)upgrade.upgradeValueMax).ToString() + ")"), ref upgrade.upgradeValueMin, ref upgrade.upgradeValueMax, 0, 50);
        if (EditorGUI.EndChangeCheck())
        {
            // Ensure that upgradeValueMin is always less than or equal to upgradeValueMax
            if (upgrade.upgradeValueMin > upgrade.upgradeValueMax)
            {
                upgrade.upgradeValueMin = upgrade.upgradeValueMax;
            }
        }

        // Display min and max values

        EditorGUILayout.EndHorizontal();
    }
}
