using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{

    public MeshFilter meshFilter;

    public void SetSkin(Mesh skin)
    {
        meshFilter.sharedMesh = skin;
    }

}
