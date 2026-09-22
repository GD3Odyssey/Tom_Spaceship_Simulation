using UnityEngine;

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
}
