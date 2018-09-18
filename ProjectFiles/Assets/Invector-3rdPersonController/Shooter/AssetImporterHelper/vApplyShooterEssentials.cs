#if UNITY_EDITOR
using UnityEditor;

public class vApplyShooterEssentials
{
    [InitializeOnLoadMethod]
    static void Apply()
    {
        EditorPrefs.SetBool("InvectorShooterImported", true);
    }
}
#endif