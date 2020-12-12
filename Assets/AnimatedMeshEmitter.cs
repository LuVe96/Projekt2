using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMeshEmitter : MonoBehaviour
{

    private SkinnedMeshRenderer skinnedMeshRenderer;
    public Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        skinnedMeshRenderer.BakeMesh(mesh);
    }
}
