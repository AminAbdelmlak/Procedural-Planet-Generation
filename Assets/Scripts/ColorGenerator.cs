using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettigs settigs;
    Texture2D texture;
    const int textureResolution = 50;

    public void UpdateSettings(ColorSettigs settigs)
    {
        this.settigs = settigs;
        if (texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    public void ElevationUpdate(MinMax elevationMinMax)
    {
        settigs.PlanetMaterial.SetVector(" _elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[textureResolution];

        for (int i = 0; i < textureResolution; i++)
        {
            colors[i] = settigs.gradient.Evaluate(i / textureResolution - 1f);
        }
        texture.SetPixels(colors);
        texture.Apply();
        settigs.PlanetMaterial.SetTexture("_Texture", texture); 
    }
}
