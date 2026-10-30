using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public float fuel = 1000f;
    public int destroyedAsteroids { get; private set; }

    [Header("Prefabs de placement")]
    public GameObject fuelPrefab;
    public GameObject asteroidPrefab;

    [Header("Placement")]
    [Min(0f)]
    public float minimumItemSpacing = 2f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            destroyedAsteroids = PlayerPrefs.GetInt("DestroyedAsteroids", 0);
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

    public void AddDestroyedAsteroid()
    {
        destroyedAsteroids++;

        PlayerPrefs.SetInt("DestroyedAsteroids", destroyedAsteroids);
        PlayerPrefs.Save();

        ShowDestroyedAsteroids();
    }

    private void ShowDestroyedAsteroids()
    {
        if (UIManager.instance == null)
            return;

        UIManager.instance.UpdateAsteroids(destroyedAsteroids, OnAsteroidsUpdated);
    }

    public void OnAsteroidsUpdated(bool updated)
    {
        Debug.Log("Asteroid counter updated : " + updated);
    }
}