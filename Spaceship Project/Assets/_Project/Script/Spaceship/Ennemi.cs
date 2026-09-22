using System;
using Unity.VisualScripting;
using UnityEngine;

public class Ennemi : MonoBehaviour
{
    public EnnemiData definition;

    private int hp = 100;
    [SerializeField] private MeshFilter model3D;
    private int uniqueID = -1;



    void OnDefinitionChanged()
    {
        Debug.Log("On validate " + this.gameObject.name);
        if (this.gameObject.scene != null)
        {
            GenerateUniqueID();
            this.gameObject.name = this.definition.title + " " + UnityEngine.Random.Range(0, 99999);
            this.model3D = this.definition.model3D;
        }

        Debug.Log("OnDefinitionChanged " + this.gameObject.name);
    }

    private void Awake()
    {
        this.hp = this.definition.hpMax;
        this.model3D = this.definition.model3D;
        Debug.Log(this.model3D);
        GenerateUniqueID();
    }

    private void GenerateUniqueID()
    {
        if (this.uniqueID > -1)
            return;
        if (UniqueIDGenerator.Instance == null)
        {
            Debug.LogError("UniqueIDGenerator introuvable dans la scène !");
            return;
        }

        this.uniqueID = UniqueIDGenerator.Instance.GetNextUniqueID();
        Debug.Log("Nouvel ID généré pour " + gameObject.name + " : " + uniqueID);
    }

    void Start()
    {
        Debug.Log("Je suis un " + definition.title + ".");
        Debug.Log("Mes HP sont " + this.hp + "/" + definition.hpMax);
    }

    void Update()
    {
        
    }
}
