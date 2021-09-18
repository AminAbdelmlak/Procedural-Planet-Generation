using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int Resolution = 10;
    public bool AutoUpdate = true;
    public enum FaceRenderMask {All, Top, Bottom, Left, Right, Front, Back}
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettigs colorSettigs;

    [HideInInspector]
    public bool ShapeSettingsFoldout;
    [HideInInspector]
    public bool ColorSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();
    
    [SerializeField, HideInInspector]
    MeshFilter[] MeshFilters;
    TerrainFace[] terrainFaces;

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettigs);

        if (MeshFilters == null || MeshFilters.Length == 0)
        {
            MeshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (MeshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("Mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                MeshFilters[i] = meshObj.AddComponent<MeshFilter>();
                MeshFilters[i].sharedMesh = new Mesh();
            }
            MeshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettigs.PlanetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, MeshFilters[i].sharedMesh, Resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            MeshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnColorSettingsUpdated()
    {
        if (AutoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }
    public void OnShapeSettingsUpdated()
    {
        if (AutoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (MeshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].CosntructMesh();
            }
        }
        colorGenerator.ElevationUpdate(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
    }
}
