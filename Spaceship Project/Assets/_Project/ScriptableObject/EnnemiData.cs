using UnityEngine;

[CreateAssetMenu(fileName = "EnnemiData", menuName = "Scriptable Objects/EnnemiData")]
public class EnnemiData : ScriptableObject
{
    public int hp = 100;
    public int hpMax = 100;
    public int power = 50;
    public int speed = 10;

    public string title;
    public MeshFilter model3D;
}
