using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[Overlay(typeof(SceneView), "Tool Spawner")]

public class MyCustomTool : Overlay
{
    public override VisualElement CreatePanelContent()
    {
        VisualElement root = new VisualElement();

        Button btnTourelle = new Button(SpawnTourelle);
        btnTourelle.text = "Spawn Tourelle";
        btnTourelle.style.height = 48;
        root.Add(btnTourelle);

        Button btnCar = new Button(SpawnCar);
        btnCar.text = "Spawn Car";
        btnCar.style.height = 48;
        root.Add(btnCar);

        Button btnSpaceship = new Button(SpawnSpaceship);
        btnSpaceship.text = "Spawn Spaceship";
        btnSpaceship.style.height = 48;
        root.Add(btnSpaceship);

        return root;
    }

    void SpawnTourelle()
    {
        Debug.Log("Tourelle");

        Scene scene = SceneManager.GetActiveScene();
        GameManager gameManager = null;

        foreach(GameObject root in scene.GetRootGameObjects())
        {
            gameManager = GameObject.FindAnyObjectByType<GameManager>();
            if (gameManager != null)
                break;
        }

        gameManager.SpawnTourelle();
    }

    void SpawnCar()
    {
        Debug.Log("Car");

        Scene scene = SceneManager.GetActiveScene();
        GameManager gameManager = null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            gameManager = GameObject.FindAnyObjectByType<GameManager>();
            if (gameManager != null)
                break;
        }

        gameManager.SpawnCar();
    }

    void SpawnSpaceship()
    {
        Debug.Log("Spaceship");

        Scene scene = SceneManager.GetActiveScene();
        GameManager gameManager = null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            gameManager = GameObject.FindAnyObjectByType<GameManager>();
            if (gameManager != null)
                break;
        }

        gameManager.SpawnSpaceship();
    }
}
