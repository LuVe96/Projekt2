using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowMaterialSetter : MonoBehaviour
{

    [SerializeField] Material glowMaterial;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material stdMaterial;

    [SerializeField] Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        stdMaterial = skinnedMeshRenderer.material;
        outline.enabled = false;
    }

   public void ChangeGlowMaterial(bool glow)
    {
        if (glow)
        {
            skinnedMeshRenderer.material = glowMaterial;
            outline.enabled = true;
        } else
        {
            skinnedMeshRenderer.material = stdMaterial;
            outline.enabled = false;
        }
    }
}
