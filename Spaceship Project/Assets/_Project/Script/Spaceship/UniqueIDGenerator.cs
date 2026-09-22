using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class UniqueIDGenerator : MonoBehaviour
{
    private static UniqueIDGenerator instance;
    public static UniqueIDGenerator Instance => instance;

    int currentUniqueId = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public int GetNextUniqueID()
    {
        currentUniqueId++;
        return currentUniqueId;
    }

    public void IncreaseBy10()
    {
        currentUniqueId = +10;
    }

    [CustomEditor(typeof(UniqueIDGenerator))]
    public class UniqueIDGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UniqueIDGenerator generator = (UniqueIDGenerator)target;

            if(GUILayout.Button("Increase By 10"))
            {
                generator.IncreaseBy10();
            }
        }
    }
}
