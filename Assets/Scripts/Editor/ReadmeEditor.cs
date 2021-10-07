using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor {
	
	static string kShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
	
	static float kSpace = 16f;
	
	static ReadmeEditor()
	{
		EditorApplication.delayCall += SelectReadmeAutomatically;
	}
	
	static void SelectReadmeAutomatically()
	{
		if (!SessionState.GetBool(kShowedReadmeSessionStateName, false ))
		{
			var readme = SelectReadme();
			SessionState.SetBool(kShowedReadmeSessionStateName, true);
			
			if (readme && !readme.loadedLayout)
			{
				// @bug : break layout.
				// LoadLayout();
				readme.loadedLayout = true;
			}
		} 
	}
	
	static void LoadLayout()
	{
		try
		{
			var assembly = typeof(EditorApplication).Assembly;
			var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
			var method = windowLayoutType.GetMethod("LoadWindowLayout", new[] { typeof(string), typeof(bool) });
			
			method.Invoke(null, new object[] { Path.Combine(Application.dataPath, "»Readme/Layout.wlt"), false });
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}
	
	[MenuItem("Help/Project Kaya README", false, 1800)]
	static Readme SelectReadme()
	{
		Object readmeObject = EditorGUIUtility.Load("Readme/ProjectKaya.asset");
		Selection.objects = new UnityEngine.Object[] { readmeObject };
			
		return (Readme)readmeObject;
	}
	
	[MenuItem("Help/URP CustomPass README", false, 1801)]
	static Readme SelectCustomPassReadme()
	{
		Object readmeObject = EditorGUIUtility.Load("Readme/URP CustomPass.asset");
		Selection.objects = new UnityEngine.Object[] { readmeObject };
			
		return (Readme)readmeObject;
	}
	
	[MenuItem("Help/Lobby Scene README", false, 1802)]
	static Readme SelectUTKTemplateReadme()
	{
		Object readmeObject = EditorGUIUtility.Load("Readme/Lobby Scene.asset");
		Selection.objects = new UnityEngine.Object[] { readmeObject };
			
		return (Readme)readmeObject;
	}
	
	protected override void OnHeaderGUI()
	{
		var readme = (Readme)target;
		Init();
		
		var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, readme.iconMaxWidth);
		
		GUILayout.BeginHorizontal("In BigTitle");
		{
			GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
			GUILayout.Label(readme.title, TitleStyle);
		}
		GUILayout.EndHorizontal();
	}
	
	public override void OnInspectorGUI()
	{
		var readme = (Readme)target;
		Init();
		
		foreach (var section in readme.sections)
		{
			if (!string.IsNullOrEmpty(section.heading))
			{
				GUILayout.Label(section.heading, HeadingStyle);
			}
			if (!string.IsNullOrEmpty(section.text))
			{
				GUILayout.Label(section.text, BodyStyle);
			}
			if (!string.IsNullOrEmpty(section.linkText))
			{
				GUILayout.Space(kSpace / 2);
				if (LinkLabel(new GUIContent(section.linkText)))
				{
					if (section.url.Contains("http"))
					{
						Application.OpenURL(section.url);
					}
					else if (section.url.Contains("guid://"))
					{
						string guid = section.url.Replace("guid://", "");
						string path = AssetDatabase.GUIDToAssetPath(guid);
						Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);

						if (asset != null)
						{
							Selection.objects = new[] { asset };
						}
					}
					else if (section.url.Contains("openscene://"))
					{
						bool isDirty = false;
						string sceneName = section.url.Replace("openscene://", "");
						var scenes = new List<Scene>();
						int openedSceneCount = SceneManager.sceneCount;
						for (int i = 0; i < openedSceneCount; i++)
						{
							var scene = SceneManager.GetSceneAt(i);
							scenes.Add(scene);

							if (scene.isDirty)
							{
								isDirty = true;
							}
						}

						if (isDirty)
						{
							EditorSceneManager.SaveModifiedScenesIfUserWantsTo(scenes.ToArray());
						}
						
						EditorSceneManager.OpenScene(sceneName);
					}
				}
			}
			GUILayout.Space(kSpace);
		}
	}
	
	
	bool m_Initialized;
	
	GUIStyle LinkStyle { get { return m_LinkStyle; } }
	[SerializeField] GUIStyle m_LinkStyle;
	
	GUIStyle TitleStyle { get { return m_TitleStyle; } }
	[SerializeField] GUIStyle m_TitleStyle;
	
	GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
	[SerializeField] GUIStyle m_HeadingStyle;
	
	GUIStyle BodyStyle { get { return m_BodyStyle; } }
	[SerializeField] GUIStyle m_BodyStyle;
	
	void Init()
	{
		if (m_Initialized)
			return;
		m_BodyStyle = new GUIStyle(EditorStyles.label);
		m_BodyStyle.wordWrap = true;
		m_BodyStyle.fontSize = 14;
		
		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 26;

		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 18;
		m_HeadingStyle.fontStyle = FontStyle.Bold;
		
		m_LinkStyle = new GUIStyle(m_BodyStyle);
		// Match selection color which works nicely for both light and dark skins
		m_LinkStyle.normal.textColor = new Color (0x00/255f, 0x78/255f, 0xDA/255f, 1f);
		m_LinkStyle.stretchWidth = false;
		
		m_Initialized = true;
	}
	
	bool LinkLabel (GUIContent label, params GUILayoutOption[] options)
	{
		var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

		Handles.BeginGUI ();
		Handles.color = LinkStyle.normal.textColor;
		Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI ();

		EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

		return GUI.Button (position, label, LinkStyle);
	}
}

