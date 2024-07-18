

using UnityEngine;
using UnityEditor;

/// <summary>
/// Needed at the Moment for the ScriptableObjectDrawer
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor
{
}