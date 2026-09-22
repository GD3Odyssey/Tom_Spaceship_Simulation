using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public float fuel = 1000f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        instance = this;
    }

    public void GetFuel(float fuel)
    {
        if (fuel == 0) return;
        this.fuel += fuel;
        SaveFuel();
        ShowFuel();
    }

    private void SaveFuel()
    {
        PlayerPrefs.SetFloat("Fuel", fuel);
    }

    private void ShowFuel()
    {
        UIManager.instance.UpdateFuel(fuel, OnFuelUpdate);
    }

    public void OnFuelUpdate(bool hasBeenFueled)
    {
            Debug.Log("Fuel updated : " + hasBeenFueled);   
    }

    public void SpawnTourelle()
    {

    }

    public void SpawnCar()
    {

    }

    public void SpawnSpaceship()
    {

    }
}
