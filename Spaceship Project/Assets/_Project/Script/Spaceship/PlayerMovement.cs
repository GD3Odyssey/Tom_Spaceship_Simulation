using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        
    }

    private void Update()
    {
        UpdateInputs();
    }

    void UpdateInputs()
    {
        if(Input.GetKey(KeyCode.W))
            MoveToDirection(Vector3.forward);

        if (Input.GetKey(KeyCode.S))
            MoveToDirection(Vector3.back);

        if (Input.GetKey(KeyCode.A))
            MoveToDirection(Vector3.left);

        if (Input.GetKey(KeyCode.D))
            MoveToDirection(Vector3.right);

        if (Input.GetKey(KeyCode.E))
            GameManager.instance.GetFuel(100f);

        if (Input.GetKey(KeyCode.Space))
        {
            Nitro(Vector3.forward);
        }
    }

    void MoveToDirection(Vector3 direction) 
    {
        this.transform.position += direction * Time.deltaTime * speed;
        FuelConsumption();
    }

    void Nitro(Vector3 direction)
    {
        this.transform.position += direction * Time.deltaTime * speed * 2f;
        FuelConsumption();
    }

    void FuelConsumption()
    {
        GameManager.instance.GetFuel(-0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            FuelPickup pickup = other.GetComponent<FuelPickup>();

            if (pickup != null)
            {
                pickup.Collect((float amount) =>
                {
                    GameManager.instance.GetFuel(amount);
                });
            }
        }
    }
}
