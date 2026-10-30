
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Spaceship Item Spawner")]
public class MyCustomTool : Overlay
{
    private enum PlacementMode
    {
        None,
        Fuel,
        Asteroid
    }

    private static PlacementMode currentMode = PlacementMode.None;

    public override VisualElement CreatePanelContent()
    {
        VisualElement root = new VisualElement();

        Button fuelButton = new Button(() => SetMode(PlacementMode.Fuel));
        fuelButton.text = "Placer Fuel";
        fuelButton.style.height = 40;
        root.Add(fuelButton);

        Button asteroidButton = new Button(() => SetMode(PlacementMode.Asteroid));
        asteroidButton.text = "Placer Astéroïde";
        asteroidButton.style.height = 40;
        root.Add(asteroidButton);

        Button stopButton = new Button(() => SetMode(PlacementMode.None));
        stopButton.text = "Arrêter le placement";
        stopButton.style.height = 30;
        root.Add(stopButton);

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;

        return root;
    }

    private static void SetMode(PlacementMode mode)
    {
        currentMode = mode;

        SceneView.RepaintAll();

        Debug.Log(mode == PlacementMode.None ? "Placement arrêté." : "Mode de placement actif : " + mode + ". Clique dans la Scene View.");
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (currentMode == PlacementMode.None)
            return;

        Event e = Event.current;

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 260, 40), EditorStyles.helpBox);
        GUILayout.Label("Placement : " + currentMode + " | Clic gauche pour placer | Echap pour annuler");
        GUILayout.EndArea();
        Handles.EndGUI();

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
        {
            SetMode(PlacementMode.None);
            e.Use();
            return;
        }

        if (e.type != EventType.MouseDown || e.button != 0 || e.alt)
            return;

        GameManager manager = Object.FindAnyObjectByType<GameManager>();

        if (manager == null)
            return;

        GameObject prefab = currentMode == PlacementMode.Fuel ? manager.fuelPrefab : manager.asteroidPrefab;

        if (prefab == null)
            return;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        Vector3 position;

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            position = hit.point;
        }
        else
        {
            Plane ground = new Plane(Vector3.up, Vector3.zero);

            if (!ground.Raycast(ray, out float distance))
                return;

            position = ray.GetPoint(distance);
        }

        if (!HasEnoughSpace(position, prefab, manager.minimumItemSpacing))
        {
            Debug.LogWarning("Placement refusé, 2 objets trop proches");
            e.Use();
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        GameObject instance = PrefabUtility.InstantiatePrefab(prefab, scene) as GameObject;

        if (instance == null)
            return;

        Undo.RegisterCreatedObjectUndo(instance, "Placer " + currentMode);

        instance.transform.position = position;

        UniqueIDGenerator generator = Object.FindAnyObjectByType<UniqueIDGenerator>();

        if (generator == null)
        {
            Undo.DestroyObjectImmediate(instance);
            e.Use();
            return;
        }

        int id = generator.GetNextUniqueID();

        if (currentMode == PlacementMode.Fuel)
        {
            FuelPickup pickup = instance.GetComponentInChildren<FuelPickup>();

            if (pickup == null)
            {
                Undo.DestroyObjectImmediate(instance);
                e.Use();
                return;
            }

            pickup.SetUniqueID(id);
        }
        else
        {
            Asteroid asteroid = instance.GetComponentInChildren<Asteroid>();

            if (asteroid == null)
            {
                Undo.DestroyObjectImmediate(instance);
                e.Use();
                return;
            }

            asteroid.SetUniqueID(id);
        }

        EditorSceneManager.MarkSceneDirty(scene);

        e.Use();
    }

    private static bool HasEnoughSpace(Vector3 position, GameObject prefab, float minimumSpacing)
    {
        float newRadius = GetRadius(prefab);

        Asteroid[] asteroids = Object.FindObjectsByType<Asteroid>(FindObjectsSortMode.None);

        foreach (Asteroid asteroid in asteroids)
        {
            if (asteroid == null || !asteroid.gameObject.scene.IsValid())
                continue;

            float existingRadius = GetRadius(asteroid.gameObject);

            float requiredDistance = Mathf.Max(minimumSpacing, newRadius + existingRadius);

            if (Vector3.Distance(position, asteroid.transform.position) < requiredDistance)
            {
                return false;
            }
        }

        FuelPickup[] fuels = Object.FindObjectsByType<FuelPickup>(FindObjectsSortMode.None);

        foreach (FuelPickup fuel in fuels)
        {
            if (fuel == null || !fuel.gameObject.scene.IsValid())
                continue;

            float existingRadius = GetRadius(fuel.gameObject);

            float requiredDistance = Mathf.Max(minimumSpacing, newRadius + existingRadius);

            if (Vector3.Distance(position, fuel.transform.position) < requiredDistance)
            {
                return false;
            }
        }

        return true;
    }

    private static float GetRadius(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        float radius = 0.5f;

        foreach (Collider col in colliders)
        {
            if (col == null)
                continue;

            radius = Mathf.Max(radius, col.bounds.extents.magnitude);
        }

        return radius;
    }
}
