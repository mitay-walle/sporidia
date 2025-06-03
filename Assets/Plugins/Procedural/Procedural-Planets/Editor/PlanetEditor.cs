// using Sporidia.Plugins.Procedural.Procedural_Planets.Runtime;
// using UnityEditor;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Sporidia.Plugins.Procedural.Procedural_Planets.Editor
// {
// 	//[CustomEditor(typeof(Planet))]
// 	public class PlanetEditor : UnityEditor.Editor
// 	{
// 		UnityEditor.Editor shapeEditor;
// 		UnityEditor.Editor colourEditor;
// 		public bool shapeSettingsFoldout;
// 		public bool colourSettingsFoldout;
//
// 		public override void OnInspectorGUI()
// 		{
// 			using (var check = new EditorGUI.ChangeCheckScope())
// 			{
// 				base.OnInspectorGUI();
// 				if (check.changed)
// 				{
// 					foreach (Planet planet in targets)
// 					{
// 						planet.GeneratePlanet();
// 					}
// 				}
// 			}
//
// 			if (GUILayout.Button("Generate Planet"))
// 			{
// 				foreach (Planet planet in targets)
// 				{
// 					planet.GeneratePlanet();
// 				}
// 			}
//
// 			if (target is Planet planet2)
// 			{
// 				DrawSettingsEditor(planet2.shapeSettings, planet2.OnShapeSettingsUpdated, ref shapeSettingsFoldout, ref shapeEditor);
// 				DrawSettingsEditor(planet2.colourSettings, planet2.OnColourSettingsUpdated, ref colourSettingsFoldout, ref colourEditor);
// 			}
// 		}
//
// 		void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref UnityEditor.Editor editor)
// 		{
// 			if (settings != null)
// 			{
// 				foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
// 				using (var check = new EditorGUI.ChangeCheckScope())
// 				{
// 					if (foldout)
// 					{
// 						CreateCachedEditor(settings, null, ref editor);
// 						editor.OnInspectorGUI();
//
// 						if (check.changed)
// 						{
// 							if (onSettingsUpdated != null)
// 							{
// 								onSettingsUpdated();
// 							}
// 						}
// 					}
// 				}
// 			}
// 		}
//
// 		void OnDisable()
// 		{
// 			if (shapeEditor) DestroyImmediate(shapeEditor);
// 			if (colourEditor) DestroyImmediate(colourEditor);
// 		}
// 	}
// }