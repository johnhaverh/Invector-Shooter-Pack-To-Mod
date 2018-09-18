using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Invector/LayersAndTags")]
public class vLayersAndTags : ScriptableObject
{
    public  List<string> layers = new List<string>
        {"Player", "Triggers", "StopMove", "HeadTrack", "BodyPart", "Enemy", "CompanionAI"};

    public List<string> tags = new List<string>
        {"Action", "AutoCrouch", "Ignore Ragdoll", "LookAt"};
}
