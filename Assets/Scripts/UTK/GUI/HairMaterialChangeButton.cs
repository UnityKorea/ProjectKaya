using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTK.Manager;

public class HairMaterialChangeButton : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material[] hairMaterials;
    private Material[] characterMaterials;
    private int materialIndex;
    private void Start()
    {
        materialIndex = 0;
        characterMaterials = skinnedMeshRenderer.materials;
        var button = GetComponent<Button>();
        if (null != button)
        {
            button.onClick.AddListener(() =>
            {
                StartCoroutine(HairChangeCo());        
            });
        }
    }
        
    protected virtual IEnumerator HairChangeCo()
    {
        yield return 0;
        characterMaterials[2] = hairMaterials[materialIndex];
        skinnedMeshRenderer.materials = characterMaterials;
        materialIndex++;
        if (materialIndex >= hairMaterials.Length)
            materialIndex = 0;
    }
}
