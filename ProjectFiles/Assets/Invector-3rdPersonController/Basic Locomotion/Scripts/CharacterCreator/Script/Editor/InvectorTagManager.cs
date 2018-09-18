using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class InvectorTagManager : EditorWindow
{
    private static InvectorTagManager m_Instance = null;

    public static InvectorTagManager FindFirstInstance()
    {
        var windows = (InvectorTagManager[])Resources.FindObjectsOfTypeAll(typeof(InvectorTagManager));
        if (windows.Length == 0)
            return null;
        return windows[0];
    }
    public GUISkin skin;
    public bool needSkin = true;
    public List<string> layers = new List<string>();
    public List<string> tags = new List<string>();
    public bool showWindowOnLoad;
    public Vector2 scroll1, scroll2;

    [InitializeOnLoadMethod]
    static void CheckLayerAndTagsWindow()
    {  
        var show = EditorPrefs.GetBool(projectName + "-Show", true);
        if( show)
           EditorApplication.delayCall+= OpenWindowWithDelay;
    }

    static void OpenWindowWithDelay()
    {
        TryShow();
    }

    [MenuItem("Invector/Open Layer and Tags Window")]
    static void OpenWindow()
    {
        TryShow(true);
    }

    private static void TryShow(bool forceShow = false)
    {
        if (Application.isPlaying) return;
      
        List<string> layers = new List<string>();
        List<string> tags = new List<string>();
        var layersAndTagsList = (vLayersAndTags[])Resources.FindObjectsOfTypeAll(typeof(vLayersAndTags));
        vLayersAndTags last = null;
        foreach (var layersAndTags in layersAndTagsList)
        {
          
            if (last)
            {
                layers.AddRange(layersAndTags.layers.Except(UnityEditorInternal.InternalEditorUtility.layers.ToList()).Except(last.layers));
                tags.AddRange(layersAndTags.tags.Except(UnityEditorInternal.InternalEditorUtility.tags.ToList()).Except(last.tags));
            }
            else
            {
                layers.AddRange(layersAndTags.layers.Except(UnityEditorInternal.InternalEditorUtility.layers.ToList()));
                tags.AddRange(layersAndTags.tags.Except(UnityEditorInternal.InternalEditorUtility.tags.ToList()));
            }

            last = layersAndTags;
        }
      
        if (layers.Count > 0 || tags.Count > 0 || forceShow)
        {
            var newWindow = m_Instance == null;
            var window = !newWindow ? m_Instance: CreateInstance<InvectorTagManager>();
            window.layers = layers;
            window.tags = tags;
            window.skin = Resources.Load("skin")as GUISkin;
            window.showWindowOnLoad = EditorPrefs.GetBool(projectName + "-Show", true);
            if (newWindow)
            window.ShowUtility();
        }
       
    }

    private void OnEnable()
    {
        m_Instance = this;
    }

    private void CreateLayer()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        if (layersProp == null || !layersProp.isArray)
        {
            Debug.LogWarning("Can't set up the layers.  It's possible the format of the layers and tags data has changed in this version of Unity.");
            Debug.LogWarning("Layers is null: " + (layersProp == null));
            return;
        }

        List<string> list = new List<string>();
        for (int a = 0; a < layersProp.arraySize; a++)
        {
            SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(a);
            list.Add(layerSP.stringValue);
        }

        for (int i = 0; i < layers.Count; i++)
        {
            if (!list.Contains(layers[i]))
            {
                bool canApplay = false;
                string layerName = "";
                for (int a = 0; a < layersProp.arraySize; a++)
                {
                    SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(a);
                    layerName = layers[i];
                    if (string.IsNullOrEmpty(layerSP.stringValue) && a > 7)
                    {
                        layerSP.stringValue = layerName;
                        list[a] = layerName;
                        Debug.Log("Invector Layer Manager info:\nSetting  up layers.  Layer " + a + " is now called " + layerName);
                        tagManager.ApplyModifiedProperties();
                        canApplay = true;
                        break;
                    }
                }
                if (!canApplay)
                {
                    Debug.LogWarning("Invector Layer Manager info:\nCan't Apply Layer " + layerName);
                }
            }
        }
    }

    private void CreateTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        if (tagsProp == null || !tagsProp.isArray)
        {
            Debug.LogWarning("Can't set up the tags.  It's possible the format of the layers and tags data has changed in this version of Unity.");
            Debug.LogWarning("Tags is null: " + (tagsProp == null));
            return;
        }
        List<string> list = new List<string>();
        for (int a = 0; a < tagsProp.arraySize; a++)
        {
            SerializedProperty _tag = tagsProp.GetArrayElementAtIndex(a);
            list.Add(_tag.stringValue);
        }
        for (int i = 0; i < tags.Count; i++)
        {
            if (!list.Contains(tags[i]))
            {
                tagsProp.arraySize++;
                SerializedProperty _tag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
                _tag.stringValue = tags[i];
                list.Add(tags[i]);
                Debug.Log("Invector Tag Manager info:\nSetting  up Tags.  Tags " + (tagsProp.arraySize - 1).ToString() + " is now called " + tags[i]);
                tagManager.ApplyModifiedProperties();
            }
        }
    }

    public static string projectName
    {
        get
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            return projectName;
        }
    }

    public void OnGUI()
    {
        minSize = new Vector3(410, 500f);
        maxSize = new Vector3(410, 500f);
        GUILayout.BeginVertical(skin.box);
        {
            EditorGUILayout.HelpBox("List of tags and layer that need to add in your project\n You can remove the ones you do not want or add all ", MessageType.Info);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(skin.box, GUILayout.MinWidth(192));
            GUILayout.Box("Layers", skin.box);
            scroll1 = GUILayout.BeginScrollView(scroll1);
            for (int i = 0; i < layers.Count; i++)
            {
                GUILayout.BeginHorizontal(skin.box);
                GUILayout.Label(layers[i], EditorStyles.whiteMiniLabel);
                bool remove = false;
                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    remove = true;
                }
                GUILayout.EndHorizontal();
                if (remove)
                {
                    layers.RemoveAt(i); break;
                }

            }
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("Apply Layers"))
            {
                CreateLayer();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(skin.box, GUILayout.MinWidth(192));
            GUILayout.Box("Tags", skin.box);
            scroll2 =GUILayout.BeginScrollView(scroll2);
            for (int i = 0; i < tags.Count; i++)
            {
                GUILayout.BeginHorizontal(skin.box);
                GUILayout.Label(tags[i], EditorStyles.whiteMiniLabel);
                bool remove = false;
                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    remove = true;
                }
                GUILayout.EndHorizontal();
                if (remove)
                {
                    tags.RemoveAt(i); break;
                }

            }
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Apply Tags"))
            {
                CreateTags();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            var _show = showWindowOnLoad;
            _show = GUILayout.Toggle(_show, "Open this window always that need additional tags and layer");
            if (_show != showWindowOnLoad)
            {
                showWindowOnLoad = _show;
                EditorPrefs.SetBool(projectName + "-Show", showWindowOnLoad);
            }
        }
       
        GUILayout.EndVertical();
        Repaint();
    }

   
}
