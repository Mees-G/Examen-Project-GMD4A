using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{

    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("AAAAAA");
            //text.SetValue(this, 100);
        }   
    }
}
