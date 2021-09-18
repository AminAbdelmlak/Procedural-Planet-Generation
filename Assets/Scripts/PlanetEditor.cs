using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor ShapeEditor;
    Editor ColorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.ShapeSettingsFoldout, ref ShapeEditor);
        DrawSettingsEditor(planet.colorSettigs, planet.OnColorSettingsUpdated, ref planet.ColorSettingsFoldout, ref ColorEditor);
    }

    void DrawSettingsEditor(Object settigs, System.Action OnSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settigs != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settigs);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settigs, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (OnSettingsUpdated != null)
                        {
                            OnSettingsUpdated();
                        }
                    }
                }
            }
        }       
    }
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
