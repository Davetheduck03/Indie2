using UnityEngine;

public class CyanTintEffect : MonoBehaviour
{
    public static CyanTintEffect Instance { get; private set; }
    public Material cyanTintMaterial;
    private bool isTintEnabled = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (isTintEnabled && cyanTintMaterial != null)
            Graphics.Blit(src, dest, cyanTintMaterial);
        else
            Graphics.Blit(src, dest);
    }

    public void ToggleTint()
    {
        isTintEnabled = !isTintEnabled;
    }
}