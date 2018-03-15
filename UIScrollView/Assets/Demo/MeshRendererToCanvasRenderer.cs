using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasRenderer)), ExecuteInEditMode]
public class MeshRendererToCanvasRenderer : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    CanvasRenderer canvasRenderer;
    // Update is called once per frame
    void Update()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        canvasRenderer = GetComponent<CanvasRenderer>();
        Mesh mesh = meshRenderer.sharedMesh;
        canvasRenderer.SetMesh(mesh);
        
        Material[] materials = meshRenderer.sharedMaterials;
        if (canvasRenderer.materialCount != materials.Length)
        {
            canvasRenderer.materialCount = materials.Length;
        }

        for (var i = 0; i < materials.Length; ++i)
        {
            canvasRenderer.SetMaterial(materials[i], i);
        }
    }

    void OnDisable()
    {
        var renderer = GetComponent<CanvasRenderer>();
        renderer.SetMesh(null);
    }
}