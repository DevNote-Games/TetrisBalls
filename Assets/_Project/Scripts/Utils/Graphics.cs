using UnityEngine;
using UnityEngine.Rendering;

public static class Graphics
{
    private const float TRANSPARENT_ALPHA = 0.6f;


    public static void SetTransparentMode(bool enabled, Material material)
    {
        if (enabled) MakeMaterialTransparent(material);
        else MakeMateriakOpaque(material);
    }


    private static void MakeMaterialTransparent(Material material)
    {
        material.SetFloat("_Surface", 1);
        material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
        material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0);

        material.renderQueue = (int)RenderQueue.Transparent;
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        material.DisableKeyword("_SURFACE_TYPE_OPAQUE");

        Color baseColor = material.GetColor("_BaseColor");
        baseColor.a = TRANSPARENT_ALPHA;
        material.SetColor("_BaseColor", baseColor);
    }

    private static void MakeMateriakOpaque(Material material)
    {
        material.SetFloat("_Surface", 0); // Opaque
        material.SetFloat("_SrcBlend", (float)BlendMode.One);
        material.SetFloat("_DstBlend", (float)BlendMode.Zero);
        material.SetFloat("_ZWrite", 1); // Включаем ZWrite

        material.renderQueue = (int)RenderQueue.Geometry;
        material.EnableKeyword("_SURFACE_TYPE_OPAQUE");
        material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
    }




}
