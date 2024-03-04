using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


    [AddComponentMenu("Mees/UI/Mask Inverter")]
    public sealed class InvertedMaskImage : MonoBehaviour, IMaterialModifier
    {
        private static readonly int _stencilComp = Shader.PropertyToID("_StencilComp");

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            var resultMaterial = new Material(baseMaterial);
            resultMaterial.SetFloat(_stencilComp, Convert.ToSingle(CompareFunction.NotEqual));
            return resultMaterial;
        }
    
}