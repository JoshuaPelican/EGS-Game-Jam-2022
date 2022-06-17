using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts : Editor
{
    [MenuItem("GameObject/Remove Missing Scripts")]
    public static void Remove()
    {
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objs)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
        }
    }
}