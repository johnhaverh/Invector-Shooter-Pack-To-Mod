#if UNITY_EDITOR
using UnityEditor;

public class vApplyMeleeEssentials
{
    [InitializeOnLoadMethod]
    static void Apply()
    {
        EditorPrefs.SetBool("InvectorMeleeImported", true);
    }
}
#endif