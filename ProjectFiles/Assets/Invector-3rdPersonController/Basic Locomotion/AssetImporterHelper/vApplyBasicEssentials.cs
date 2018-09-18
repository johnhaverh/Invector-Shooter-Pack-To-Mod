#if UNITY_EDITOR
using UnityEditor;

public class vApplyBasicEssentials
{
    [InitializeOnLoadMethod]
    static void Apply()
    {
        EditorPrefs.SetBool("InvectorBasicImported", true);
    }
}
#endif