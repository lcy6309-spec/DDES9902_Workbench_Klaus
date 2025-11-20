using UnityEngine;

public class ClaySmoothnessInstant : MonoBehaviour
{
    [Header("Clay MeshRenderer")]
    public MeshRenderer clayRenderer; 

    [Header("Smoothness Settings")]
    [Range(0f, 1f)]
    public float targetSmoothness = 0.8f; 

    private Material clayMaterial;

    void Start()
    {
        if (clayRenderer != null)
        {
            clayMaterial = clayRenderer.material;
        }
    }

    public void MakeSmooth()
    {
        if (clayMaterial == null)
        {
            Debug.LogWarning("Please assign Clay Renderer in the Inspector!");
            return;
        }

        clayMaterial.SetFloat("_Smoothness", targetSmoothness);
    }
}
