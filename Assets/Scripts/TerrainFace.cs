using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace 
{
    ShapeGenerator shapeGenerator;

    Mesh Mesh;
    int Resolution;
    Vector3 LocalUp;

    Vector3 AxisA;
    Vector3 AxisB;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.Mesh = mesh;
        this.Resolution = resolution;
        this.LocalUp = localUp;

        AxisA = new Vector3(LocalUp.y, LocalUp.z, LocalUp.x);
        AxisB = Vector3.Cross(LocalUp, AxisA);
    }

    public void CosntructMesh()
    {
        Vector3[] vertices = new Vector3[Resolution * Resolution];
        int[] triangles = new int[(Resolution - 1) * (Resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++)
            {
                int i = x + y * Resolution;
                Vector2 percent = new Vector2(x, y) / (Resolution - 1);
                Vector3 PointOnUnitCube = LocalUp + (percent.x - .5f) * 2 * AxisA + (percent.y - .5f) * 2 * AxisB;
               
                Vector3 PointOnUnitSphere = PointOnUnitCube.normalized;
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(PointOnUnitSphere);

                if (x != Resolution - 1 && y != Resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + Resolution + 1;
                    triangles[triIndex + 2] = i + Resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + Resolution + 1;

                    triIndex += 6;
                }
            }
        }
        Mesh.Clear();
        Mesh.vertices = vertices;
        Mesh.triangles = triangles;
        Mesh.RecalculateNormals();
    }
}
