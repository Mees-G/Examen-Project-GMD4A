using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pinwheel.Griffin.TextureTool;

namespace Pinwheel.Griffin
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class SmoothPolarisTerrain : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private GStylizedTerrain terrain;
        public GStylizedTerrain Terrain
        {
            get
            {
                return terrain;
            }
            private set
            {
                terrain = value;
            }
        }

        public bool applySmoothing = true;
        public float angle = 90;

        private void OnEnable()
        {
            Terrain = GetComponent<GStylizedTerrain>();
            if (Terrain != null)
            {                
                Terrain.PostProcessHeightMap += OnPostProcessHeightMap;
            }
        }

        private void OnDisable()
        {
            if (Terrain != null)
            {                
                Terrain.PostProcessHeightMap -= OnPostProcessHeightMap;
            }          
        }      

        private void OnPostProcessHeightMap(Texture2D heightMap)
        {
            if(applySmoothing)
                ProcessSmoothing();
        }

        public void ProcessSmoothing()
        {
            GTerrainChunk[] chunks = GetComponentsInChildren<GTerrainChunk>();

            foreach (GTerrainChunk chunk in chunks)
            {
                NormalSolver.RecalculateNormals(chunk.MeshFilterComponent.sharedMesh, angle);
            }
        }
    }
}
