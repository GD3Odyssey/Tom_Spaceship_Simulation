using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 2f;
    public float deceleration = 1f;
    public float maxSpeed = 7f;

    [Header("Rotation")]
    public float barrellRollSpeed = 80f;
    public float pitchSpeed = 80f;

    [Header("Burst")]
    public float burstMultiplier = 2f;
    public float burstFuel = 5f;
    public float fuelConsumption = 5f;

    public bool IsBursting { get; private set; }

    private Rigidbody rb;

    private float currentSpeed;

    private float forwardInput;
    private float yawInput;
    private float rollInput;
    private float pitchInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        RotateShip();
    }

    private void UpdateInputs()
    {
        forwardInput = 0f;

        if (Input.GetKey(KeyCode.W))
            forwardInput += 1f;

        if (Input.GetKey(KeyCode.S))
            forwardInput -= 1f;

        yawInput = 0f;

        if (Input.GetKey(KeyCode.A))
            yawInput -= 1f;

        if (Input.GetKey(KeyCode.D))
            yawInput += 1f;

        rollInput = 0f;

        if (Input.GetKey(KeyCode.Q))
            rollInput += 1f;

        if (Input.GetKey(KeyCode.E))
            rollInput -= 1f;

        pitchInput = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            pitchInput -= 1f;

        if (Input.GetKey(KeyCode.DownArrow))
            pitchInput += 1f;

        IsBursting = Input.GetKey(KeyCode.Space) && currentSpeed > 0f;

        if (IsBursting)
        {
            GameManager.instance.GetFuel(-burstFuel * Time.deltaTime);
        }
    }

    private void UpdateMovement()
    {
        if (forwardInput != 0f)
        {
            currentSpeed += forwardInput * acceleration * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        float effectiveMaxSpeed = IsBursting ? maxSpeed * burstMultiplier : maxSpeed;

        currentSpeed = Mathf.Clamp(currentSpeed, -effectiveMaxSpeed, effectiveMaxSpeed);

        if (IsBursting && currentSpeed > 0f)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * burstMultiplier * Time.fixedDeltaTime, effectiveMaxSpeed);
        }

        Vector3 movement = transform.forward * currentSpeed;

        rb.linearVelocity = movement;

        if (!Mathf.Approximately(currentSpeed, 0f))
        {
            FuelConsumption();
        }
    }

    private void RotateShip()
    {
        Vector3 rotation = new Vector3(pitchInput * pitchSpeed, yawInput * barrellRollSpeed, rollInput * barrellRollSpeed);

        Quaternion deltaRotation = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void FuelConsumption()
    {
        float consumption = fuelConsumption * Mathf.Abs(currentSpeed) * Time.fixedDeltaTime;

        GameManager.instance.GetFuel(-consumption);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fuel"))
            return;

        FuelPickup pickup = other.GetComponent<FuelPickup>();

        if (pickup == null)
            pickup = other.GetComponentInParent<FuelPickup>();

        if (pickup == null)
            pickup = other.GetComponentInChildren<FuelPickup>();

        if (pickup == null)
            return;

        pickup.Collect((float amount) =>
        {
            GameManager.instance.GetFuel(amount);
        });
    }

    private void OnCollisionEnter(Collision collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponentInParent<Asteroid>();

        if (asteroid == null)
            return;

        if (!IsBursting)
            return;

        asteroid.DestroyAsteroid();
    }
}