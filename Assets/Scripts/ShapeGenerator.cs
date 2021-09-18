using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;

    public MinMax elevationMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 PointOnUnitShpere)
    {
        float FirstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            FirstLayerValue = noiseFilters[0].Evaluate(PointOnUnitShpere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = FirstLayerValue;
            }
        }
        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float Mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? FirstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(PointOnUnitShpere) * Mask; 
            }
        }
        elevation = settings.PlanetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return PointOnUnitShpere * elevation;
    }
}
