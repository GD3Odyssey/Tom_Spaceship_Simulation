using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 10f;
    public float deceleration = 5f;
    public float maxSpeed = 20f;
    public float barrellRollSpeed = 80f;
    private float currentSpeed;

    [Header("Burst")]
    public float burstMultiplier = 2f;
    public float burstFuel = 0.1f;
    public float fuelConsumption = 0.05f;

    private void Update()
    {
        UpdateInputs();
        UpdateMovement();
    }

    private void UpdateInputs()
    {
        float forwardInput = 0f;

        if (Input.GetKey(KeyCode.W))
            forwardInput += 1f;

        if (Input.GetKey(KeyCode.S))
            forwardInput -= 1f;

        if (forwardInput != 0f)
        {
            // acceleration
            currentSpeed += forwardInput * acceleration * Time.deltaTime;
        }
        else
        {
            // deceleration
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        float yawInput = 0f;

        if (Input.GetKey(KeyCode.A))
            yawInput -= 1f;

        if (Input.GetKey(KeyCode.D))
            yawInput += 1f;

        float rollInput = 0f;

        if (Input.GetKey(KeyCode.Q))
            rollInput += 1f;

        if (Input.GetKey(KeyCode.E))
            rollInput -= 1f;

        RotateShip(yawInput, rollInput);

        // burst
        if (Input.GetKey(KeyCode.Space) && currentSpeed > 0f)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * burstMultiplier * Time.deltaTime, maxSpeed * burstMultiplier);

            GameManager.instance.GetFuel(-burstFuel * Time.deltaTime);
        }
    }

    private void RotateShip(float yawInput, float rollInput)
    {
        Vector3 rotation = new Vector3(0f, yawInput * barrellRollSpeed * Time.deltaTime, rollInput * barrellRollSpeed * Time.deltaTime);

        transform.Rotate(rotation, Space.Self);
    }

    private void UpdateMovement()
    {
        if (Mathf.Approximately(currentSpeed, 0f))
            return;

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        FuelConsumption();
    }

    private void FuelConsumption()
    {
        float consumption = fuelConsumption * Mathf.Abs(currentSpeed) * Time.deltaTime;

        GameManager.instance.GetFuel(-consumption);
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